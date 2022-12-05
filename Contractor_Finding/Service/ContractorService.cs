using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class ContractorService : IContractorService
    {
        private readonly ContractorFindingContext contractorFindingContext;

        //Constructor
        public ContractorService(ContractorFindingContext contractorFindingContext)
        {
            this.contractorFindingContext = contractorFindingContext;
        }

        //create
        public string CreateContractor(ContractorDetail contractorDetail)
        {
            var id = contractorFindingContext.TbUsers.Where(u => u.UserId == contractorDetail.ContractorId).FirstOrDefault();
            var checklicense = contractorFindingContext.ContractorDetails.Where(u => u.License == contractorDetail.License).FirstOrDefault();
            if (id != null && checklicense == null)
            {
                var license = contractorDetail.License.Trim();
                if (license == string.Empty)
                {
                    return null;
                   
                }
                else
                {
                    contractorFindingContext.ContractorDetails.Add(contractorDetail);
                    contractorFindingContext.SaveChanges();
                    return "Successful!";
                }
            }
            else
            {
                return null;
            }
        }

        //RETRIEVE
        public List<ContractorDisplay> GetContractorDetails()
        {
            List<ContractorDisplay> contractors = (from c in contractorFindingContext.ContractorDetails
                                                   join g in contractorFindingContext.TbGenders on
                                                   c.Gender equals g.GenderId
                                                   join user in contractorFindingContext.TbUsers on c.ContractorId equals user.UserId
                                                   //from e in contractorFindingDemoContext.ContractorDetails
                                                   join h in contractorFindingContext.ServiceProvidings on
                                                   c.Services equals h.ServiceId
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
                                                       FirstName = user.FirstName,
                                                       LastName = user.LastName,
                                                       EmailId = user.EmailId,
                                                       PhoneNumber = c.PhoneNumber
                                                   }).ToList();
            return contractors;
        }

        //UPDATE
        public string updateContractorDetails(ContractorDetail contractorDetail)
        {

            var contractorobj = contractorFindingContext.ContractorDetails.Where(c => c.ContractorId == contractorDetail.ContractorId).FirstOrDefault();
            var licenseobj = contractorFindingContext.ContractorDetails.Where(c => c.License == contractorDetail.License).FirstOrDefault();
            var licensecon = contractorobj.License;
            if (contractorobj != null && licenseobj != null)
            {

                contractorobj.CompanyName = contractorDetail.CompanyName;
                contractorobj.Gender = contractorDetail?.Gender;
                contractorobj.Services = contractorDetail?.Services;
                contractorobj.PhoneNumber = contractorDetail?.PhoneNumber;
                contractorobj.Lattitude = contractorDetail?.Lattitude;
                contractorobj.Longitude = contractorDetail?.Longitude;
                contractorobj.Pincode = contractorDetail.Pincode;
                if (contractorDetail.CompanyName != null && contractorDetail.Pincode != 0 && contractorDetail.ContractorId == contractorobj.ContractorId && contractorDetail.License == licensecon)
                {
                    contractorFindingContext.SaveChanges();
                    return "sucessfully Updated!";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        //DELETE
        public bool DeleteContractor(ContractorDetail contractorDetail)
        {
            ContractorDetail contractor = contractorFindingContext.ContractorDetails.Where(x => x.License == contractorDetail.License).FirstOrDefault()!;
            contractorFindingContext.ContractorDetails.Remove(contractor);
            contractorFindingContext.SaveChanges();
            return true;
        }
    }
}
