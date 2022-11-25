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
        public bool CreateCustomer(TbCustomer tbCustomer)
        {
            if (tbCustomer.RegistrationNo == null)
            {
                return false;
            }
            else
            {
                contractorFindingContext.TbCustomers.Add(tbCustomer);
                contractorFindingContext.SaveChanges();
                return true;
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
        public bool UpdateCustomerDetails(TbCustomer tbCustomer)
        {
            using (var context = new ContractorFindingContext())
            {
                var customer = context.TbCustomers.Where(x => x.RegistrationNo == tbCustomer.RegistrationNo).FirstOrDefault();
                if (customer != null)
                {
                    customer.LandSqft = tbCustomer.LandSqft;
                    customer.BuildingType = tbCustomer.BuildingType;
                    customer.Lattitude = tbCustomer.Lattitude;
                    customer.Longitude = tbCustomer.Longitude;
                    customer.Pincode = tbCustomer.Pincode;
                    if (customer.LandSqft != null && customer.RegistrationNo != null && customer.Pincode != null)
                    {
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
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
