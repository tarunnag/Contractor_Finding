using Azure;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Persistence;
using Service;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace API.Controllers
{
    [Authorize]//(Policy = "User")]
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IUserService userService;
        private readonly IJWTAuthentication jwtAuthentication;

        //constructor
        public UserController(ContractorFindingContext contractordemoContext, IUserService userService, IJWTAuthentication jwtAuthentication)
        {
            this.contractorFindingContext = contractordemoContext;
            this.userService = userService;
            this.jwtAuthentication = jwtAuthentication;
        }

        ////for get user details
        //// GET: api/<ContractorController>
        [HttpGet]
        public JsonResult Getuserdetails()
        {
            try
            {
                var details = userService.GetUserDetails();
                return new JsonResult(details);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //for user registration
        // POST api/<ContractorController>
        [HttpPut]
        public JsonResult RegisterUser(Registration registration)
        {

            try
            {
                var userexist= userService.checkExistUser(registration);

                if (userexist != null)
                {
                    var details = userService.Register(registration);
                    if (details != null)
                    {
                        return new JsonResult(new CrudStatus() { Status = true, Message = "Registration Successful!" });
                    }
                    else
                    {
                        return new JsonResult(new CrudStatus() { Status = false, Message = "registration failed" });
                    }
                }
                else
                {
                    return new JsonResult(new CrudStatus() { Status = false, Message = "Mail ID is already existing" });
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            
        }



        //////for user login 
        //[HttpPost("login")]
        //public JsonResult LoginUser(Login login)
        //{
        //    try
        //    {
        //        var details = userService.Login(login);
        //        if (details == null)
        //        {
        //            return new JsonResult(new CrudStatus() { Status = true, Message = "Login Successfull!" });
        //        }
        //        return new JsonResult(new CrudStatus() { Status = false, Message = "LoginFailed" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult(ex.Message);
        //    }
        //}
        [AllowAnonymous]

        [HttpGet("login")]
        public IActionResult Login(string username, string password)
        {
            try
            {
                // var IsUserAuthenticated = userService.AuthenticateUser(username,password);

                string? token = this.jwtAuthentication.Authenticate(username, password);
                setTokenCookie(token);
                if (token == null)
                    return Unauthorized();
                else
                    return Ok(token);
                //return new JsonResult(new CrudStatus() { Status = true, Message = "Login Successfull!" });




            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost]
        [Route("refresh-token")]
        public IActionResult RefreshToken(string accessToken)
        {
            if (accessToken == "")
            {
                return BadRequest("Invalid client request");
            }


            //string? refreshToken = tokenModel.RefreshToken;

            var principal = jwtAuthentication.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            string? username = jwtAuthentication.ValidateJwtToken(accessToken);
            TbUser user = jwtAuthentication.getUserByRefreshToken(username);

            if (user == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string? newAccessToken = this.jwtAuthentication.Authenticate(username, user.Password);
            // var newRefreshToken = GenerateRefreshToken();
            setTokenCookie(newAccessToken);
            if (newAccessToken == null)
                return Unauthorized();
            else
                return Ok(newAccessToken);


        }

        //for forgot password
        [HttpPost("forgotpassword")]
        public JsonResult ForgotPassword(Login login)
        {
            try
            {
                var details = userService.forgotpassword(login);
                if (details != null)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "Password Updated" });
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Not Updated" });             
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        // for DELETE 
        [HttpDelete]
        public JsonResult Delete(TbUser user)
        {
            try
            {
                var details = userService.DeleteUser(user);
                if (details == true)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "Deleted successful!" });
                }
                return new JsonResult(new CrudStatus() { Status = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
