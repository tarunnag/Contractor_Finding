using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Repository;
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
        private readonly IMapper _mapper;

        //constructor
        public UserController(ContractorFindingContext contractordemoContext, IUserService userService, IMapper mapper, IGenerateToken generateToken):base(contractordemoContext)
        {
            this.userService = userService;
            this.generateToken = generateToken;
            _mapper=mapper;
        }

        //for get user details
        // GET: api/<ContractorController>
        /// <summary>
        /// Get the details of the users 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns></returns>
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

        //For Versioning
        /// <summary>
        /// Get the details of the users with versioning 2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("2")]
        [Route("V2")]
        public ActionResult<List<UserDisplayV2>> GetUsers2()
        {
            var users = userService.GetUsers().Select(a => _mapper.Map<UserDisplayV2>(a));
            return Ok(users);
        }

        /// <summary>
        /// Get the details of the users with versioning 3
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("3")]
        [Route("V3")]
        public JsonResult GetUsers3()
        {
            var user = new MapperConfiguration(cfg => cfg.CreateProjection<TbUser, UserDisplayV2>()
            .ForMember(dto => dto.Usertype, conf =>
             conf.MapFrom(ol => ol.TypeUserNavigation.Usertype1)));
            return new JsonResult(contractorFindingContext.TbUsers.ProjectTo<UserDisplayV2>(user).ToList());
        }

        /// <summary>
        /// Get the details of the users with versioning 4
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("4")]
        [Route("V4")]
        public List<UserDisplayV2> GetUsers4()
        {
            List<UserDisplayV2> users = AutoMapper<Userview, UserDisplayV2>.Maplist(userService.GetUsers());
            return users;

        }

        //for user registration
        /// <summary>
        /// Adding the user to the database (table Tb_User)
        /// here password will encrypt
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Login using emailid and password, generate token
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
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
        /// <summary>
        /// At the time of login , token is generated for users(Customer and Contractor)
        /// this token have some span of time.
        /// token will refresh.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Refresh_Token")]
        public IActionResult RefreshToken(string accessToken)
        {
            if (accessToken == "")
            {
                return BadRequest("Invalid client request");
            }

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
        /// <summary>
        /// for changing password by using valid emailID,after password will encrypt
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
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
