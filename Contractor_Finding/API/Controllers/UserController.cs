using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Service;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IUserService userService;
        private readonly IGenerateToken generateToken;
        //constructor
        public UserController(ContractorFindingContext contractordemoContext, IUserService userService, IGenerateToken generateToken)
        {
            this.contractorFindingContext = contractordemoContext; 
            this.userService = userService;
            this.generateToken = generateToken;
        }
     

        //for get user details
        // GET: api/<ContractorController>
        [HttpGet]
        [Authorize]
        public JsonResult Getuserdetails()
        {
            try
            {
                //var details = userService.GetUserDetails();
                //return new JsonResult(details);
                return new JsonResult(userService.GetUserDetails().ToList());
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

                if (userexist == true)
                {
                    var details = userService.Register(registration);
                    if (details == true)
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

        ////for user login //
        [HttpPost("login")]
        public JsonResult LoginUser(TbUser login)
        {
            try
            {
                var details = userService.Login(login);
                if (details != null)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = details });
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "LoginFailed" });

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        //Refresh Token
        [HttpPost]
        [Route("Refresh_Token")]
        public IActionResult RefreshToken(string accessToken)
        {
            if (accessToken == "")
            {
                return BadRequest("Invalid client request");
            }


            //string? refreshToken = tokenModel.RefreshToken;

            var principal = generateToken.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            string? username = generateToken.ValidateJwtToken(accessToken);
            TbUser user = userService.GetUserDetails().Where(x => x.EmailId == username).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string? newAccessToken = generateToken.GenerateNewToken(user);

            if (newAccessToken == null)
                return Unauthorized();
            else
                return Ok(newAccessToken);


        }



        //for forgot password
        [HttpPost("forgotpassword")]
        public JsonResult ForgotPassword(Registration login)
        {
            try
            {
                var details = userService.forgotpassword(login);
                if (details == true)
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
      

    }
}
