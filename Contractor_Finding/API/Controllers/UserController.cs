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
    public class UserController : BaseController
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IUserService userService;
        private readonly IGenerateToken generateToken;
        private const string Sessionkey = "UserId";

        //constructor
        public UserController(ContractorFindingContext contractordemoContext, IUserService userService, IGenerateToken generateToken):base(contractordemoContext)
        {
            this.userService = userService;
            this.generateToken = generateToken;
        }
     

        //for get user details
        // GET: api/<ContractorController>
        [HttpGet]
        [Authorize]
        public ActionResult Getuserdetails([FromQuery] Pagination pageParams)
        {
            loginID(Sessionkey);
            try
            {
                return Ok (userService.GetUserDetails(pageParams).ToList());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //for user registration
        // POST api/<ContractorController>
        [HttpPut]
        public IActionResult RegisterUser(Registration registration)
        {

            try
            {
                    var details = userService.Register(registration);
                    if (details == true)
                    {
                        return Ok(new CrudStatus() { Status = true, Message = "Registration Successful!" });
                    }
                    return Ok(new CrudStatus() { Status = false, Message = "registration failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        ////for user login //
        [HttpPost("login")]
        public IActionResult LoginUser(TbUser login)
        {
            try
            {
                var details = userService.Login(login);
                if (details != null)
                {
                    HttpContext.Session.SetInt32(Sessionkey, details.Item2);
                    loginID(Sessionkey);
                    return Ok(new CrudStatus() { Status = true, Message = details.Item1 });
                }
                return Ok(new CrudStatus() { Status = false, Message = "LoginFailed" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        public IActionResult ForgotPassword(Registration login)
        {
            try
            {
                var details = userService.forgotpassword(login);
                if (details == true)
                {
                    return Ok(new CrudStatus() { Status = true, Message = "Password Updated" });
                }
                return Ok(new CrudStatus() { Status = false, Message = "Not Updated" });             
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        // for DELETE 
        [HttpDelete]
        public IActionResult Delete(TbUser user)
        {
            try
            {
                var details = userService.DeleteUser(user);
                if (details == true)
                {
                    return Ok(new CrudStatus() { Status = true, Message = "Deleted successful!" });
                }
                return Ok(new CrudStatus() { Status = false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
      

    }
}
