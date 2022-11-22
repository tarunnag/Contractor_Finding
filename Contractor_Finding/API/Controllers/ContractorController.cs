using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorController : ControllerBase
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IContractorServive contractorService;

        //Constructor
        public ContractorController(ContractorFindingContext contractorFindingContext, IContractorServive contractorService)
        {
            this.contractorFindingContext = contractorFindingContext;
            this.contractorService = contractorService;
        }

        //create
        [HttpPut]
        public JsonResult CreateContractor(ContractorDetail contractorDetail)
        {
            try
            {
                return new JsonResult(contractorService.CreateContractor(contractorDetail));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //RETRIVE
        [HttpGet]
        public JsonResult GetContractorDetails()
        {
            try
            {
                return new JsonResult(contractorService.GetContractorDetails().ToList());
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //UPDATE
        [HttpPost]
        public JsonResult UpdateContractor(ContractorDetail contractorDetail)
        {
            try
            {
                return new JsonResult(contractorService.updateContractorDetails(contractorDetail));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //DELETE
        [HttpDelete]
        public JsonResult DeleteContractor(ContractorDetail contractorDetail)
        {
            try
            {
                return new JsonResult(contractorService.DeleteContractor(contractorDetail));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
 
        }
    }
}
