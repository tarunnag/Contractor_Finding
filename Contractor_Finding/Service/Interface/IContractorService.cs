using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IContractorService
    {
        //string CreateContractor(ContractorDetail contractorDetail);
        public string CreateContractor(ContractorDetail contractorDetail);
        List<ContractorDisplay> GetContractorDetails();
        //string updateContractorDetails(ContractorDetail contractorDetail);
        public string updateContractorDetails(ContractorDetail contractorDetail);

        public bool DeleteContractor(ContractorDetail contractorDetail);
    }
}
