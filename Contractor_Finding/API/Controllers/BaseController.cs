using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private const string Sessionkey = "userId";

        public BaseController(ContractorFindingContext contractorFindingContext)
        {
            this.contractorFindingContext = contractorFindingContext;
        }
   
        [ApiExplorerSettings(IgnoreApi = true)]
        public void loginID(string sessionkey)
        {
            var test = HttpContext.Session.GetInt32(sessionkey);
        }
    }
}
