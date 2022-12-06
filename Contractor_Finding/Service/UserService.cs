using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly ContractorFindingContext contractorFindingContext;
        private readonly IEncrypt encrypt;
      
        private readonly IGenerateToken generateToken;
        public UserService(ContractorFindingContext contractorFindingContext, IEncrypt encrypt, IGenerateToken generateToken)
        {
            this.contractorFindingContext = contractorFindingContext;
            this.encrypt = encrypt;
        
            this.generateToken = generateToken;
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
                                          FirstName = u.FirstName,
                                          LastName = u.FirstName,
                                          EmailId = u.EmailId,
                                          Password=u.Password,
                                          PhoneNumber = u.PhoneNumber,
                                          UserTypeName = ud.Usertype1,
                                          CreatedDate = u.CreatedDate,
                                          UpdatedDate = u.UpdatedDate,
                                          Active = u.Active,
                                          TypeUser = u.TypeUser,
                                      }).ToList();
            return user;
        }

        public string checkExistUser(TbUser tbUser)
        {
            var email = contractorFindingContext.TbUsers.Where(e => e.EmailId == tbUser.EmailId).FirstOrDefault();
            if (email == null)
            {
                return "user doesnot exist";
            }
            return "already exist";
        }

       
        //for Registration
        public string Register(Registration registration)
        {

            var email = contractorFindingContext.TbUsers.Where(e => e.EmailId == registration.EmailId).FirstOrDefault();
            if (email == null)
            {
                string encryptedPassword = encrypt.EncodePasswordToBase64(registration.Password);
                registration.CreatedDate = DateTime.Now;
                registration.UpdatedDate = null;
                registration.Active = true;
                string passwordconfirm = encrypt.EncodePasswordToBase64(registration.confirmationPassword);
                registration.Password = encryptedPassword;
                registration.confirmationPassword = passwordconfirm;
                if (registration.Password == registration.confirmationPassword)
                {
                    contractorFindingContext.TbUsers.Add(registration);
                    contractorFindingContext.SaveChanges();
                    return "successfully registered";
                }
                else
                {
                    return "registration failed";
                }
            }
            else
            {
                return "registration failed";
            }
        }
      

      
        ////for Login
        public string Login(TbUser login)
        {

            Encrypt decrypt = new Encrypt();
            string checkingpassword = encrypt.EncodePasswordToBase64(login.Password);

            var myUser = contractorFindingContext.TbUsers.
                FirstOrDefault(u => u.EmailId == login.EmailId
                && u.Password == checkingpassword);
            if (myUser != null)
            {
                var token = generateToken.GenerateNewToken(myUser);
                return token;
            }
            return null!;


        }

     

        //for forgotpassword case
        public string forgotpassword(Login login)
        {

            var userWithSameEmail = contractorFindingContext.TbUsers.Where(m => m.EmailId == login.EmailId).SingleOrDefault();
            if (userWithSameEmail == null)
            {
                return "Updation Failed";
            }
            else
            {

                string encrptnewpassword = encrypt.EncodePasswordToBase64(login.Password);
                string encrptconfirmpassword = encrypt.EncodePasswordToBase64(login.confirmPassword);
                if (encrptnewpassword == encrptconfirmpassword)
                {
                    userWithSameEmail.Password = encrptconfirmpassword;
                    userWithSameEmail.UpdatedDate = DateTime.Now;
                    contractorFindingContext.Entry(userWithSameEmail).State = EntityState.Modified;
                    contractorFindingContext.SaveChanges();
                    return "Successful!";
                }
                else
                {
                    return "Updation Failed";
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
