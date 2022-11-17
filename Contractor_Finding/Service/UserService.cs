using Domain;
using Domain.Models;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly ContractorFindingContext contractorFindingContext;


        //private readonly IEncrypt encrypt;
        public UserService(ContractorFindingContext contractorFindingContext)
        {
            this.contractorFindingContext = contractorFindingContext;
        }

        //For Display
        public List<UserDisplay> GetUserDetails()
        {
            List<UserDisplay> user = (from u in contractorFindingContext.TbUsers
                                      join ud in contractorFindingContext.UserTypes on
                                      u.TypeUser equals ud.TypeId
                                      select new UserDisplay
                                      {
                                          UserId = u.UserId,
                                          TypeUser = ud.Usertype1,
                                          FirstName = u.FirstName,
                                          LastName = u.FirstName,
                                          EmailId = u.EmailId,
                                          PhoneNumber = u.PhoneNumber,
                                      }).ToList();
            return user;
        }

        //for Registration
        public string Register(Registration registration)
        {
            Encrypt encrypt = new Encrypt();
            string encryptedPassword = encrypt.EncodePasswordToBase64(registration.Password);
            string passwordconfirm = encrypt.EncodePasswordToBase64(registration.confirmationPassword);
            registration.Password = encryptedPassword;
            registration.confirmationPassword = passwordconfirm;
            if (registration.Password == registration.confirmationPassword)
            {
                contractorFindingContext.TbUsers.Add(registration);
                contractorFindingContext.SaveChanges();
                return "registration sucessfull";
            }
            else
            {
                return "password not matched";
            }
        }

        //for Login
        public string Login(Login login)
        {
            Encrypt decrypt = new Encrypt();
            string checkingpassword = decrypt.EncodePasswordToBase64(login.Password);
            var myUser = contractorFindingContext.TbUsers.
                FirstOrDefault(u => u.EmailId == login.EmailId
                && u.Password == checkingpassword);
            if (myUser == null)
            {
                return "login failed";
            }
            else
            {
                return "login successful";
            }

        }

        //for forgotpassword case
        public string forgotpassword(Login login)
        {
            var userWithSameEmail = contractorFindingContext.TbUsers.Where(m => m.EmailId == login.EmailId).SingleOrDefault();
            if (userWithSameEmail == null)
            {
                return "enter the correct the emailid";
            }
            else
            {
                Encrypt encrypt = new Encrypt();
                string encrptnewpassword = encrypt.EncodePasswordToBase64(login.Password);
                string encrptconfirmpassword = encrypt.EncodePasswordToBase64(login.confirmPassword);
                if (encrptnewpassword == encrptconfirmpassword)
                {
                    userWithSameEmail.Password = encrptconfirmpassword;
                    contractorFindingContext.SaveChanges();
                    return "password change successful";
                }
                else
                {
                    return "password wrong";
                }

            }
        }

        //for delete deatils
        public bool DeleteUser(TbUser user)
        {
            contractorFindingContext.TbUsers.Remove(user);
            contractorFindingContext.SaveChanges();
            return true;
        }

    }
}
