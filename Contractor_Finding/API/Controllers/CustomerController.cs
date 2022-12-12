using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service;
using Service.Interface;

namespace API.Controllers
{
    [Authorize(Policy = "customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly ICustomerService customerService;
        private const string Sessionkey = "UserId";
        //Constructor 
        public CustomerController(ContractorFindingContext contractorFindingContext, ICustomerService customerService):base(contractorFindingContext)
        {
            this.customerService = customerService;
        }

        //create
        [HttpPut]
        public IActionResult CreateContractor(TbCustomer tbCustomer)
        {
            try
            {
                var customer = customerService.CreateCustomer(tbCustomer);
                if (customer == true)
                {
                    return Ok(new CrudStatus() { Status = true, Message = "Added Successful!" });
                }
                return Ok(new CrudStatus() { Status = false, Message = "Failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //RETRIEVE
        [HttpGet]
        public ActionResult GetCustomerDetails()
        {
            try
            {
                return Ok(customerService.GetCustomerDetails().ToList());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //UPDATE
        [HttpPost]
        public IActionResult UpdateCustomerDetails(TbCustomer tbCustomer)
        {
            try
            {
                var contractor = customerService.UpdateCustomerDetails(tbCustomer);
                if (contractor == true)
                {
                    return Ok(new CrudStatus() { Status = true, Message = "Successfully Updated" });
                }
                return Ok(new CrudStatus() { Status = false, Message = "Updation Failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETE
        [HttpDelete]
        public IActionResult DeleteCustomer(TbCustomer tbCustomer)
        {
            try
            {
                var customer = customerService.DeleteCustomer(tbCustomer);
                if (customer == true)
                {
                    return Ok(new CrudStatus() { Status = true, Message = "Deleted successfully!" });
                }
                return Ok(new CrudStatus() { Status = false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //SerachingContractor
        [HttpGet("Pincode")]
        public ActionResult SearchBypincode(int pin)
        {
            try
            {
                return Ok(customerService.SearchBypincode(pin).ToList());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Sending Notification
        [HttpPost("SendingToContractor")]
        public IActionResult SendNotification(long phonenumber, string registration, int id)
        {
            try
            {
                return Ok(customerService.SendMessage(phonenumber, registration, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }      
    }
}
