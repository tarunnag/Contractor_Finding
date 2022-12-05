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

namespace Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly SendMessage sms;
        private readonly ContractorService _contractorservice;

        public CustomerService(ContractorFindingContext contractorFindingContext)
        {
            this.contractorFindingContext = contractorFindingContext;
            this.sms = new SendMessage();
            _contractorservice = new ContractorService(contractorFindingContext);
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
                    customer.CustomerId= tbCustomer.CustomerId;
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
            else
            {
                return "failed";
            }
        }

        //search
        public List<ContractorDisplay> SearchBypincode(int pincode)
        {
            return _contractorservice.GetContractorDetails().Where(x => x.Pincode == pincode).ToList();
        }
    }
}
