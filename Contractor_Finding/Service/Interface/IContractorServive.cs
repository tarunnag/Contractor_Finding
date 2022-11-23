using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IContractorServive
    {
        //string CreateContractor(ContractorDetail contractorDetail);
        public bool CreateContractor(ContractorDetail contractorDetail);
        List<ContractorDisplay> GetContractorDetails();
        //string updateContractorDetails(ContractorDetail contractorDetail);
        public bool updateContractorDetails(ContractorDetail contractorDetail);

        public bool DeleteContractor(ContractorDetail contractorDetail);
    }
}
