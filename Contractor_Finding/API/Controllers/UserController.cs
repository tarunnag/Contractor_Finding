using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IUserService userService;

        //constructor
        public UserController(ContractorFindingContext contractordemoContext, IUserService userService)
        {
            this.contractorFindingContext = contractordemoContext;
            this.userService = userService;
        }

        //for get user details
        // GET: api/<ContractorController>
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
        [HttpPost]
        public JsonResult RegisterUser(Registration registration)
        {
            try
            {
                var userWithSameEmail = contractorFindingContext.TbUsers.Where(m => m.EmailId == registration.EmailId).SingleOrDefault();
                if (ModelState.IsValid)
                {
                    if (userWithSameEmail == null)
                    {
                        var details = userService.Register(registration);
                        return new JsonResult(details);
                    }
                    else
                    {
                        return new JsonResult("email already in use");
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            return new JsonResult("registration success!");
        }

        //for user login 
        [HttpPost("login")]
        public JsonResult LoginUser(Login login)
        {
            try
            {
                var details = userService.Login(login);
                return new JsonResult(details);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //for forgot password
        [HttpPost("forgotpassword")]
        public JsonResult ForgotPassword(Login login)
        {
            try
            {
                var details = userService.forgotpassword(login);
                return new JsonResult(details);
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
                return new JsonResult(details);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}
