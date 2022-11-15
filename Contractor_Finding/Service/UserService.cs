using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService:IUserService
    {
        private readonly ContractorFindingContext _contractorFindingContext;
        public UserService(ContractorFindingContext contractorFindingContext)
        {
            _contractorFindingContext = contractorFindingContext;
        }
        public List<UserDetail> GetUserDetails()
        {
            List<UserDetail> userDetails = new List<UserDetail>();
            return (_contractorFindingContext.UserDetails.ToList());
        }
        public bool Create(UserDetail userDetail)
        {
            bool result = false;
            var email = _contractorFindingContext.UserDetails.Find(userDetail.MailId);
            if (email != null)
            {
                return result;
            }
            else
            {
                _contractorFindingContext.UserDetails.Add(userDetail);
                var password = _contractorFindingContext.UserDetails.Find(userDetail.Password);
                if (password != null)
                {
                    Encrypt encrypt = new Encrypt();
                    encrypt.Decrypt_Password(userDetail.Password);
                }
                _contractorFindingContext.SaveChanges();
                return true;
            }
        }
        public bool DeleteUser(UserDetail userDetail)
        {
            _contractorFindingContext.UserDetails.Remove(userDetail);
            _contractorFindingContext.SaveChanges();

            return true;
        }

    }
}
