using Domain.Models;
using Domain;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;

namespace Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ContractorFindingContext contractorFindingContext;

        public CustomerService(ContractorFindingContext contractorFindingContext)
        {
            this.contractorFindingContext = contractorFindingContext;
        }

        //create
        public string CreateCustomer(TbCustomer tbCustomer)
        {
            var registrationID = contractorFindingContext.TbCustomers.Where(r => r.RegistrationNo == tbCustomer.RegistrationNo).FirstOrDefault();
            if (registrationID == null && tbCustomer.LandSqft != null)
            {
                var ID = tbCustomer.RegistrationNo.Trim();
                if (ID == string.Empty)
                {
                    return null;
                }
                else
                {
                    contractorFindingContext.TbCustomers.Add(tbCustomer);
                    contractorFindingContext.SaveChanges();
                    return "Successfully Added";
                }
            }
            else
            {
                return null;
            }
        }

        //RETRIEVE
        public List<CustomerDisplay> GetCustomerDetails()
        {
            List<CustomerDisplay> customers = (from c in contractorFindingContext.TbCustomers
                                               join b in contractorFindingContext.TbBuildings on
                                               c.BuildingType equals b.Id
                                               select new CustomerDisplay
                                               {
                                                   LandSqft = c.LandSqft,
                                                   RegistrationNo = c.RegistrationNo,
                                                   BuildingType = b.Building,
                                                   Lattitude = c.Lattitude,
                                                   Longitude = c.Longitude,
                                                   Pincode = c.Pincode,

                                               }).ToList();
            return customers;
        }

        //UPDATE
        public string UpdateCustomerDetails(TbCustomer tbCustomer)
        {
            {
                var customer = contractorFindingContext.TbCustomers.Where(x => x.RegistrationNo == tbCustomer.RegistrationNo).FirstOrDefault();
                if (customer != null)
                {
                    customer.LandSqft = tbCustomer.LandSqft;
                    customer.BuildingType = tbCustomer.BuildingType;
                    customer.Lattitude = tbCustomer.Lattitude;
                    customer.Longitude = tbCustomer.Longitude;
                    customer.Pincode = tbCustomer.Pincode;
                    if (customer.LandSqft != null && customer.RegistrationNo != null && customer.Pincode != null)
                    {
                        contractorFindingContext.SaveChanges();
                        return "Successfully Updated!";
                    }
                    return null;
                }
                else
                {
                    return null;
                }

            }
        }

        //DELETE
        public bool DeleteCustomer(TbCustomer tbCustomer)
        {
            TbCustomer customer = contractorFindingContext.TbCustomers.Where(c => c.RegistrationNo == tbCustomer.RegistrationNo).FirstOrDefault()!;
            contractorFindingContext.TbCustomers.Remove(customer);
            contractorFindingContext.SaveChanges();
            return true;
        }
    }
}
