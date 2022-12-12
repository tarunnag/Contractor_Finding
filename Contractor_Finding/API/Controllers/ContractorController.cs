using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service.Interface;
using Twilio.TwiML.Voice;

namespace API.Controllers
{
    [Authorize(Policy = "contractor")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorController : BaseController
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IContractorService contractorService;
        private const string Sessionkey = "UserId";

        //Constructor
        public ContractorController(ContractorFindingContext contractorFindingContext, IContractorService contractorService):base(contractorFindingContext)
        {
            this.contractorService = contractorService;
        }

        //[Authorize(Policy = "contractor")]
        //create
        [HttpPut]
        
        public IActionResult CreateContractor(ContractorDetail contractorDetail)
        {
            try
            {
                var contractor = contractorService.CreateContractor(contractorDetail);
                if (contractor == true)
                {
                    return Ok(new CrudStatus() { Status = true, Message = "Successful!" });
                }
                return Ok(new CrudStatus() { Status = false, Message = "Failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Policy = "customer")]
        //RETRIVE
        [HttpGet]
        public ActionResult<List<ContractorDisplay>> GetContractorDetails([FromQuery] Pagination pageParams)
        {
            try
            {
                return Ok(contractorService.GetContractorDetails(pageParams).ToList());

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

       // [Authorize(Policy = "contractor")]
        //UPDATE
        [HttpPost]

        public IActionResult UpdateContractor(ContractorDetail contractorDetail)
        {
            try
            {
                var contractor = contractorService.updateContractorDetails(contractorDetail);
                if (contractor == true && contractorDetail.License!=null)
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

        //[Authorize(Policy = "contractor")]
        //DELETE
        [HttpDelete]
        public IActionResult DeleteContractor(ContractorDetail contractorDetail)
        {
            try
            {
                var contractor = contractorService.DeleteContractor(contractorDetail);
                if (contractor == true)
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
