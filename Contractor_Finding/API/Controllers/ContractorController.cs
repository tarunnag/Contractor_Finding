using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service.Interface;

namespace API.Controllers
{
    [Authorize(Policy = "contractor")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorController : ControllerBase
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IContractorService contractorService;


        //Constructor
        public ContractorController(ContractorFindingContext contractorFindingContext, IContractorService contractorService)
        {
            this.contractorFindingContext = contractorFindingContext;
            this.contractorService = contractorService;
        }

        //[Authorize(Policy = "contractor")]
        //create
        [HttpPut]
        
        public JsonResult CreateContractor(ContractorDetail contractorDetail)
        {
            try
            {
                var contractor = contractorService.CreateContractor(contractorDetail);
                if (contractor == true)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "Successful!" });
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Failed" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //[Authorize(Policy = "customer")]
        //RETRIVE
        [HttpGet]

        public JsonResult GetContractorDetails([FromQuery] Pagination pageParams)
        {
            try
            {
                return new JsonResult(contractorService.GetContractorDetails(pageParams).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

       // [Authorize(Policy = "contractor")]
        //UPDATE
        [HttpPost]

        public JsonResult UpdateContractor(ContractorDetail contractorDetail)
        {
            try
            {
                var contractor = contractorService.updateContractorDetails(contractorDetail);
                if (contractor == true && contractorDetail.License!=null)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "Successfully Updated" });
                }
                else
                return new JsonResult(new CrudStatus() { Status = false, Message = "Updation Failed" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //[Authorize(Policy = "contractor")]
        //DELETE
        [HttpDelete]
        public JsonResult DeleteContractor(ContractorDetail contractorDetail)
        {
            try
            {
                var contractor = contractorService.DeleteContractor(contractorDetail);
                if (contractor == true)
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
