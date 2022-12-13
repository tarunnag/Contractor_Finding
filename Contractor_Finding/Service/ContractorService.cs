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
        public bool CreateContractor(ContractorDetail contractorDetail)
        {
            var id = contractorFindingContext.TbUsers.Where(u => u.UserId == contractorDetail.ContractorId).FirstOrDefault();
            var checklicense = contractorFindingContext.ContractorDetails.Where(u => u.License == contractorDetail.License).FirstOrDefault();
            if (id != null && checklicense == null)
            {
                var license = contractorDetail.License.Trim();
                if (license != string.Empty)
                {
                    contractorFindingContext.ContractorDetails.Add(contractorDetail);
                    contractorFindingContext.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        //RETRIEVE
        public List<ContractorDisplay> GetContractorDetails(Pagination pageParams)
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
            switch (pageParams.OrderBy)
            {
                case "CompanyName":
                    contractors = contractors.OrderBy(on => on.CompanyName).ToList();
                    break;
                case "ContractorId":
                    contractors = contractors.OrderBy(on => on.ContractorId).ToList();
                    break;
                case "Gender":
                    contractors = contractors.OrderBy(on => on.Gender).ToList();
                    break;
                case "License":
                    contractors = contractors.OrderBy(on => on.License).ToList();
                    break;
                case "Services":
                    contractors = contractors.OrderBy(on => on.Services).ToList();
                    break;
                case "Lattitude":
                    contractors = contractors.OrderBy(on => on.Lattitude).ToList();
                    break;
                case "Longitude":
                    contractors = contractors.OrderBy(on => on.Longitude).ToList();
                    break;
                case "Pincode":
                    contractors = contractors.OrderBy(on => on.Pincode).ToList();
                    break;
                case "FirstName":
                    contractors = contractors.OrderBy(on => on.FirstName).ToList();
                    break;
                case "LastName":
                    contractors = contractors.OrderBy(on => on.LastName).ToList();
                    break;
                case "EmailId":
                    contractors = contractors.OrderBy(on => on.EmailId).ToList();
                    break;
                case "PhoneNumber":
                    contractors = contractors.OrderBy(on => on.PhoneNumber).ToList();
                    break;
                default:
                    contractors = contractors.OrderBy(on => on.ContractorId).ToList();
                    break;
            }
            contractors = contractors.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                .Take(pageParams.PageSize).ToList();
            return contractors;
        }

        //UPDATE
        public async Task<ContractorDetail> updateContractorDetails(ContractorDetail contractorDetail)
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
                    await contractorFindingContext.SaveChangesAsync();
                    return contractorDetail;
                }
                return null;
            }
            return null;
        }

        //DELETE
        public bool DeleteContractor(ContractorDetail contractorDetail)
        {

            ContractorDetail contractor = contractorFindingContext.ContractorDetails.Where(x => x.License == contractorDetail.License).FirstOrDefault()!;
            if (contractor != null)
            {
                contractorFindingContext.ContractorDetails.Remove(contractor);
                contractorFindingContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
