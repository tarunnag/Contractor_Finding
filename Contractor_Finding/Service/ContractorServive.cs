using Domain;
using Domain.Models;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ContractorServive : IContractorServive
    {
        private readonly ContractorFindingContext contractorFindingContext;

        //Constructor
        public ContractorServive(ContractorFindingContext contractorFindingContext)
        {
            this.contractorFindingContext = contractorFindingContext;
        }
        //CREATE Contractor Details
        public string CreateContractor(ContractorDetail contractorDetail)
        {
            contractorFindingContext.ContractorDetails.Add(contractorDetail);
            contractorFindingContext.SaveChanges();
            return "Successful!";
        }

        //RETRIEVE
        public List<ContractorDisplay> GetContractorDetails()
        {
            List<ContractorDisplay> contractors = (from c in contractorFindingContext.ContractorDetails
                                                   join g in contractorFindingContext.TbGenders on
                                                   c.Gender equals g.GenderId
                                                   from e in contractorFindingContext.ContractorDetails
                                                   join h in contractorFindingContext.ServiceProvidings on
                                                   e.Services equals h.ServiceId
                                                   select new ContractorDisplay
                                                   {
                                                       ContractorId = c.ContractorId,
                                                       CompanyName = c.CompanyName,
                                                       Gender = g.GenderType,
                                                       License = c.License,
                                                       Services = h.ServiceName,
                                                       Lattitude = c.Lattitude,
                                                       Longitude = c.Longitude,
                                                       Pincode = c.Pincode,

                                                   }).ToList();
            return contractors;
        }

        //Update
        //public string updateContractor(ContractorDetail contractorDetail)
        //{
        //    //if (contractorDetail.CompanyName != null && contractorDetail.Gender != null && contractorDetail.Lattitude != null &&
        //    //    contractorDetail.Longitude != null && contractorDetail.License != null && contractorDetail.Pincode != null &&
        //    //    contractorDetail.Services != null)
        //    //{

        //    using (var context = new ContractorFindingContext())
        //    {
        //        contractorFindingContext.ContractorDetails.Update(contractorDetail);
        //        contractorFindingContext.SaveChanges();
        //        return "update successfull";
        //    }


        //return "some fields are not mentioned";


    }
}
