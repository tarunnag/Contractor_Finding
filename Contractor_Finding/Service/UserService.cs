﻿using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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


        private readonly IEncrypt encrypt;
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

        public bool checkExistUser(TbUser tbUser)
        {
            var email= contractorFindingContext.TbUsers.Where(e=>e.EmailId==tbUser.EmailId).FirstOrDefault();
            if (email == null)
            {
                return false;
            }
            return true;
        }

        //public bool Register(Registration registration)
        //{
        //    registration.Password= encrypt.EncodePasswordToBase64(registration.Password);
        //    registration.CreatedDate = DateTime.Now;
        //    registration.UpdatedDate = null;
        //    registration.Active = true;
        //    contractorFindingContext.TbUsers.Add(registration);
        //    return true;
        //}

        //for Registration
        public bool Register(Registration registration)
        {
            Encrypt encrypt = new Encrypt();
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
                return true;
            }
            else
            {
                return false;
            }
        }

        //for Login
        public bool Login(Login login)
        {

            Encrypt decrypt = new Encrypt();
            string checkingpassword = decrypt.EncodePasswordToBase64(login.Password);
            var myUser = contractorFindingContext.TbUsers.
                FirstOrDefault(u => u.EmailId == login.EmailId
                && u.Password == checkingpassword);
            if (myUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        //for forgotpassword case

        //public bool forgotpassword(Login login)
        //{
        //    TbUser user = contractorFindingContext.TbUsers.Where(a => a.EmailId == login.EmailId).SingleOrDefault();
        //    user.Password = encrypt.EncodePasswordToBase64(login.Password);
        //    user.UpdatedDate = DateTime.Now;
        //    contractorFindingContext.Entry(user).State = EntityState.Modified;
        //    contractorFindingContext.SaveChanges();
        //    return true;
        //}

        //for forgotpassword case
        public bool forgotpassword(Login login)
        {

            var userWithSameEmail = contractorFindingContext.TbUsers.Where(m => m.EmailId == login.EmailId).SingleOrDefault();
            if (userWithSameEmail == null)
            {
                return false;
            }
            else
            {
                Encrypt encrypt = new Encrypt();
                TbUser tbUser = new TbUser();
                string encrptnewpassword = encrypt.EncodePasswordToBase64(login.Password);
                string encrptconfirmpassword = encrypt.EncodePasswordToBase64(login.confirmPassword);
                if (encrptnewpassword == encrptconfirmpassword)
                {
                    userWithSameEmail.Password = encrptconfirmpassword;
                    userWithSameEmail.UpdatedDate = DateTime.Now;
                    contractorFindingContext.Entry(userWithSameEmail).State = EntityState.Modified;
                    contractorFindingContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
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
