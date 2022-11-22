using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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
                                                       PhoneNumber = c.PhoneNumber,

                                                   }).ToList();
            return contractors;
        }

        //UPDATE
        public string updateContractorDetails(ContractorDetail contractorDetail)
        {
            using (var context = new ContractorFindingContext())
            {
                var contractorobj = context.ContractorDetails.Where(c => c.ContractorId == contractorDetail.ContractorId).FirstOrDefault();
                if (contractorobj != null)
                {
                    //context.ContractorDetails.Remove(contractorDetail);
                    contractorobj.CompanyName= contractorDetail.CompanyName;
                    contractorobj.Gender= contractorDetail?.Gender;
                    contractorobj.Services= contractorDetail?.Services;
                    contractorobj.PhoneNumber = contractorDetail?.PhoneNumber;
                    contractorobj.Lattitude= contractorDetail?.Lattitude;
                    contractorobj.Longitude = contractorDetail?.Longitude;
                    contractorobj.Pincode = contractorDetail.Pincode;
                    context.SaveChanges();
                    return "successfully updated";
                }
                else
                {
                    return "not updated ";
                }
            }
        }

        //DELETE
        public string DeleteContractor(ContractorDetail contractorDetail)
        {
            ContractorDetail contractor = contractorFindingContext.ContractorDetails.Where(x => x.License == contractorDetail.License).FirstOrDefault()!;
            contractorFindingContext.ContractorDetails.Remove(contractor);
            contractorFindingContext.SaveChanges();
            return "Deleted";
        }

    }
}
