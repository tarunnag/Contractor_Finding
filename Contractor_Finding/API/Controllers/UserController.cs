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

        //for user login 
        [HttpPost("login")]
        public JsonResult LoginUser(Login login)
        {
            try
            {               
                var details = userService.Login(login);
                if(details!=null)
                {
                    return new JsonResult(new CrudStatus() { Status= true, Message="Login Successfull!"});
                }
                return new JsonResult(new CrudStatus() { Status=false,Message="LoginFailed"});
                
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

    }
}
