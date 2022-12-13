using Domain.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICustomerService
    {
        bool CreateCustomer(TbCustomer tbCustomer);
        List<CustomerDisplay> GetCustomerDetails(Pagination pageParams);
        Task<TbCustomer> UpdateCustomerDetails(TbCustomer tbCustomer);
        bool DeleteCustomer(TbCustomer tbCustomer);
        public List<ContractorDisplay> SearchBypincode(int pincode, Pagination pageParams);
        string SendMessage(long phonenumber, string reggistration, int id);
    }
}
