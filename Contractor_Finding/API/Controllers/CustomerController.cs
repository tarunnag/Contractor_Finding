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
        [HttpPut]
        public JsonResult CreateContractor(TbCustomer tbCustomer)
        {
            try
            {
                var customer = customerService.CreateCustomer(tbCustomer);
                if (customer == true)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "Added Successful!" });
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Failed" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //RETRIEVE
        [HttpGet]
        public JsonResult GetCustomerDetails([FromQuery] Pagination pageParams)
        {
            try
            {
                return new JsonResult(customerService.GetCustomerDetails(pageParams).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //UPDATE
        [HttpPost]
        public JsonResult UpdateCustomerDetails(TbCustomer tbCustomer)
        {
            try
            {
                var contractor = customerService.UpdateCustomerDetails(tbCustomer);
                if (contractor == true)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "Successfully Updated" });
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Updation Failed" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //DELETE
        [HttpDelete]
        public JsonResult DeleteCustomer(TbCustomer tbCustomer)
        {
            try
            {
                var customer = customerService.DeleteCustomer(tbCustomer);
                if (customer == true)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "Deleted successfully!" });
                }
                return new JsonResult(new CrudStatus() { Status = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //SerachingContractor
        [HttpGet("Pincode")]
        public JsonResult SearchBypincode(int pin, Pagination pageParams)
        {
            try
            {
                return new JsonResult(customerService.SearchBypincode(pin, pageParams).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //Sending Notification
        [HttpPost("SendingToContractor")]
        public JsonResult SendNotification(long phonenumber, string registration, int id)
        {
            try
            {
                return new JsonResult(customerService.SendMessage(phonenumber, registration, id));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }      
    }
}
