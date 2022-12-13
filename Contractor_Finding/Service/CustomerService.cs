using Domain.Models;
using Domain;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;
using Twilio.TwiML.Voice;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly SendMessage sms;
        private readonly ContractorService _contractorservice;

        //Constructor
        public CustomerService(ContractorFindingContext contractorFindingContext)
        {
            this.contractorFindingContext = contractorFindingContext;
            this.sms = new SendMessage();
            _contractorservice = new ContractorService(contractorFindingContext);
        }

        //create
        public bool CreateCustomer(TbCustomer tbCustomer)
        {
            var registrationID = contractorFindingContext.TbCustomers.Where(r => r.RegistrationNo == tbCustomer.RegistrationNo).FirstOrDefault();
            if (registrationID == null && tbCustomer.LandSqft != null)
            {
                var ID = tbCustomer.RegistrationNo.Trim();
                if (ID != string.Empty)
                {
                    contractorFindingContext.TbCustomers.Add(tbCustomer);
                    contractorFindingContext.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        //RETRIEVE
        public List<CustomerDisplay> GetCustomerDetails(Pagination pageParams)
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
            switch (pageParams.OrderBy)
            {
                case "LandSqft":
                    customers = customers.OrderBy(on => on.LandSqft).ToList();
                    break;
                case "RegistrationNo":
                    customers = customers.OrderBy(on => on.RegistrationNo).ToList();
                    break;
                case "BuildingType":
                    customers = customers.OrderBy(on => on.BuildingType).ToList();
                    break;
                case "Lattitude":
                    customers = customers.OrderBy(on => on.Lattitude).ToList();
                    break;
                case "Longitude":
                    customers = customers.OrderBy(on => on.Longitude).ToList();
                    break;
                case "Pincode":
                    customers = customers.OrderBy(on => on.Pincode).ToList();
                    break;
                default:
                    customers = customers.OrderBy(on => on.RegistrationNo).ToList();
                    break;
            }
            customers = customers.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                .Take(pageParams.PageSize).ToList();
            return customers;
        }

        //UPDATE
        public async Task<TbCustomer> UpdateCustomerDetails(TbCustomer tbCustomer)
        {
            var customer = contractorFindingContext.TbCustomers.Where(x => x.RegistrationNo == tbCustomer.RegistrationNo).FirstOrDefault();
            if (customer != null)
            {
                customer.LandSqft = tbCustomer.LandSqft;
                customer.BuildingType = tbCustomer.BuildingType;
                customer.Lattitude = tbCustomer.Lattitude;
                customer.Longitude = tbCustomer.Longitude;
                customer.Pincode = tbCustomer.Pincode;
                customer.CustomerId = tbCustomer.CustomerId;
                if (customer.LandSqft != null && customer.LandSqft != 0 && customer.RegistrationNo != null && customer.Pincode != null)
                {
                    await contractorFindingContext.SaveChangesAsync();
                    return tbCustomer;
                }
                return null;
            }
            return null;
        }

        //DELETE
        public bool DeleteCustomer(TbCustomer tbCustomer)
        {
            TbCustomer customer = contractorFindingContext.TbCustomers.Where(c => c.RegistrationNo == tbCustomer.RegistrationNo).FirstOrDefault()!;
            if (customer != null)
            {
                contractorFindingContext.TbCustomers.Remove(customer);
                contractorFindingContext.SaveChanges();
                return true;
            }
            return false;
        }

        //send message
        public string SendMessage(long phonenumber, string reggistration, int id)
        {
            var phone = contractorFindingContext.ContractorDetails.Where(c => c.PhoneNumber == phonenumber).FirstOrDefault();
            var registrationid = contractorFindingContext.TbCustomers.Where(c => c.RegistrationNo == reggistration).FirstOrDefault();
            var customer = contractorFindingContext.TbUsers.Where(a => a.UserId == id).FirstOrDefault();
            var custid = contractorFindingContext.TbCustomers.Where(a => a.CustomerId == id).FirstOrDefault();
            if (phone != null && registrationid != null && custid != null && customer != null)
            {
                string message2 = customer.FirstName + " " + customer.LastName + " \n phone number : " + customer.PhoneNumber + " \n Emailid:  " + customer.EmailId + "\n Land squarefeet: " + registrationid.LandSqft + "\n Pincode:  " + registrationid.Pincode;
                sms.SendMessageToContractor(message2, phonenumber);
                return "Message sended";
            }
            return "failed";
        }

        //search
        public List<ContractorDisplay> SearchBypincode(int pincode, Pagination pageParams)
        {
          
            return  _contractorservice.GetContractorDetails(pageParams).Where(x => x.Pincode == pincode).ToList();
            

        }
    }
}
