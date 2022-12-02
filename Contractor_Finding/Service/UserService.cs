using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IEncrypt encrypt;
        private readonly IConfiguration _config;
        public UserService(ContractorFindingContext contractorFindingContext, IEncrypt encrypt, IConfiguration config)
        {
            this.contractorFindingContext = contractorFindingContext;
            this.encrypt = encrypt;
            _config = config;
        }

        //For Display
        public List<UserDisplay> GetUserDetails()
        {
            List<UserDisplay> user = (from u in contractorFindingContext.TbUsers
                                      join ud in contractorFindingContext.UserTypes on
                                      u.TypeUser equals ud.TypeId
                                      select new UserDisplay
                                      {
                                          UserId = u.UserId,
                                          FirstName = u.FirstName,
                                          LastName = u.FirstName,
                                          EmailId = u.EmailId,
                                          Password=u.Password,
                                          PhoneNumber = u.PhoneNumber,
                                          UserTypeName = ud.Usertype1,
                                          CreatedDate = u.CreatedDate,
                                          UpdatedDate = u.UpdatedDate,
                                          Active = u.Active,
                                          TypeUser = u.TypeUser,
                                      }).ToList();
            return user;
        }

        public string checkExistUser(TbUser tbUser)
        {
            var email = contractorFindingContext.TbUsers.Where(e => e.EmailId == tbUser.EmailId).FirstOrDefault();
            if (email == null)
            {
                return "user doesnot exist";
            }
            return "already exist";
        }

       
        //for Registration
        public string Register(Registration registration)
        {

            var email = contractorFindingContext.TbUsers.Where(e => e.EmailId == registration.EmailId).FirstOrDefault();
            if (email == null)
            {
                string encryptedPassword = encrypt.EncodePasswordToBase64(registration.Password);
                registration.CreatedDate = DateTime.Now;
                registration.UpdatedDate = null;
                registration.Active = true;
                string passwordconfirm = encrypt.EncodePasswordToBase64(registration.confirmationPassword);
                registration.Password = encryptedPassword;
                registration.confirmationPassword = passwordconfirm;
                if (registration.Password == registration.confirmationPassword)
                {
                    contractorFindingContext.TbUsers.Add(registration);
                    contractorFindingContext.SaveChanges();
                    return "successfully registered";
                }
                else
                {
                    return "registration failed";
                }
            }
            else
            {
                return "registration failed";
            }
        }
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
        public string? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);


            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],

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

      



        ////for Login
        public string Login(Login login)
        {

            Encrypt decrypt = new Encrypt();
            string checkingpassword = encrypt.EncodePasswordToBase64(login.Password);

            var myUser = contractorFindingContext.TbUsers.
                FirstOrDefault(u => u.EmailId == login.EmailId
                && u.Password == checkingpassword);
            if (myUser != null)
            {
                var token = GenerateToken(myUser);
                return token;
            }
            return null!;


        }

        public string GenerateToken(TbUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            UserType role = contractorFindingContext.UserTypes.Where(x => x.TypeId == user.TypeUser).FirstOrDefault()!;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.EmailId!),
                 new Claim(ClaimTypes.NameIdentifier,user.Password!),
                  new Claim(ClaimTypes.Role, role.Usertype1!),

            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       

        //for forgotpassword case
        public string forgotpassword(Login login)
        {

            var userWithSameEmail = contractorFindingContext.TbUsers.Where(m => m.EmailId == login.EmailId).SingleOrDefault();
            if (userWithSameEmail == null)
            {
                return "Updation Failed";
            }
            else
            {

                string encrptnewpassword = encrypt.EncodePasswordToBase64(login.Password);
                string encrptconfirmpassword = encrypt.EncodePasswordToBase64(login.confirmPassword);
                if (encrptnewpassword == encrptconfirmpassword)
                {
                    userWithSameEmail.Password = encrptconfirmpassword;
                    userWithSameEmail.UpdatedDate = DateTime.Now;
                    contractorFindingContext.Entry(userWithSameEmail).State = EntityState.Modified;
                    contractorFindingContext.SaveChanges();
                    return "Successful!";
                }
                else
                {
                    return "Updation Failed";
                }

            }
        }

        //for delete deatils
        public bool DeleteUser(TbUser user)
        {
            contractorFindingContext.TbUsers.Remove(user);
            contractorFindingContext.SaveChanges();
            return true;
        }

    }
}
