using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Adding Customer details into database(table Tb_Customer)
        /// </summary>
        /// <param name="tbCustomer"></param>
        /// <returns></returns>
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
        /// <summary>
        /// get all details of the Customer
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCustomerDetails([FromQuery] Pagination pageParams)
        {
            try
            {
                return Ok(customerService.GetCustomerDetails(pageParams).ToList());

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //UPDATE
        /// <summary>
        /// updating the details of customer with registered RegistrationID,can't edit registrationID
        /// </summary>
        /// <param name="tbCustomer"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CustomerUpdate")]
        public async Task<IActionResult> Post(TbCustomer tbCustomer)
        {
            try
            {
                await customerService.UpdateCustomerDetails(tbCustomer);
                if (tbCustomer != null)
                {
                    return Ok(new CrudStatus() { Status = true, Message = "Successfully Updated" });
                }
                return Ok(new CrudStatus() { Status = false, Message = "Updation Failed" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //DELETE
        /// <summary>
        /// Delete the details of customer by using RegistrationID
        /// </summary>
        /// <param name="tbCustomer"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Getting details of contractor by using Pincode which is already registered in Contractor details.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="pageParams"></param>
        /// <returns></returns>
        [HttpGet("Pincode")]
        public ActionResult SearchBypincode(int pin, Pagination pageParams)
        {
            try
            {
                return Ok(customerService.SearchBypincode(pin, pageParams).ToList());

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Sending Notification
        /// <summary>
        /// sending the SMS as customer details using registered contractor phomnenumber,RegistrationID and CustomerID of customeR
        /// </summary>
        /// <param name="phonenumber"></param>
        /// <param name="registration"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
