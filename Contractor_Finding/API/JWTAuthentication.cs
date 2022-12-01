using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API
{
    public interface IJWTAuthentication
    {
        public string? Authenticate(string username, string password);
        public string? ValidateJwtToken(string token);
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
        public TbUser getUserByRefreshToken(string username);
        //public RefreshToken GenerateRefreshToken(string ipAddress);
    }
    public class JWTAuthentication : IJWTAuthentication
    {
        //key declaration
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public JWTAuthentication(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public string? Authenticate(string username, string password)
        {
            List<UserDisplay> users = _userService.GetUserDetails();
            //auth failed - creds incorrect
            if (!_userService.AuthenticateUser(username, password))
            {
                return null;
            }
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["AppSettings:Key"]);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256);

            var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("fullName", username),
            new Claim(ClaimTypes.Role, users[0].UserRole),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Aud, Convert.ToString(_configuration["AppSettings:Audience"])),
            new Claim(JwtRegisteredClaimNames.Iss, Convert.ToString(_configuration["AppSettings:Issuer"]))
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMonths(2),
                signingCredentials: credentials
                );
            return tokenHandler.WriteToken(token);
        }


        public string? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Key"]);


            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["AppSettings:Issuer"],
                    ValidAudience = _configuration["AppSettings:Audience"],
                    
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.Select(x => x.Value).First();

                // return user id from JWT token if validation successful
                return userId;
            }
            catch (Exception ex)
            {
                // return null if validation fails
                return null;
            }
        }

        public TbUser getUserByRefreshToken(string username)
        {
            var user = _userService.GetById(username);

            return user;

        }
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["AppSettings:Issuer"],
                ValidAudience = _configuration["AppSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        //public AuthenticateResponse RefreshToken(string token, string ipAddress)
        //{
        //    var user = getUserByRefreshToken(token);
        //    var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        //    if (refreshToken.IsRevoked)
        //    {
        //        // revoke all descendant tokens in case this token has been compromised
        //        revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
        //        _context.Update(user);
        //        _context.SaveChanges();
        //    }

        //    if (!refreshToken.IsActive)
        //        throw new AppException("Invalid token");

        //    // replace old refresh token with a new one (rotate token)
        //    var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
        //    user.RefreshTokens.Add(newRefreshToken);

        //    // remove old refresh tokens from user
        //    removeOldRefreshTokens(user);

        //    // save changes to db
        //    _context.Update(user);
        //    _context.SaveChanges();

        //    // generate new jwt
        //    var jwtToken = _jwtUtils.GenerateJwtToken(user);

        //    return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        //}

    }
}
