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

        //Constractor
        public UserService(ContractorFindingContext contractorFindingContext, IEncrypt encrypt, IGenerateToken generateToken)
        {
            this.contractorFindingContext = contractorFindingContext;
            this.encrypt = encrypt;
        
            this.generateToken = generateToken;
        }

        //For Display
        public List<UserDisplay> GetUserDetails(Pagination? pageParams=null)
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
            if (pageParams != null)
            {
                switch (pageParams.OrderBy)
                {
                    case "UserId":
                        user = user.OrderBy(on => on.UserId).ToList();
                        break;
                    case "FirstName":
                        user = user.OrderBy(on => on.FirstName).ToList();
                        break;
                    case "LastName":
                        user = user.OrderBy(on => on.LastName).ToList();
                        break;
                    case "EmailId":
                        user = user.OrderBy(on => on.EmailId).ToList();
                        break;
                    case "Password":
                        user = user.OrderBy(on => on.Password).ToList();
                        break;
                    case "UserTypeName":
                        user = user.OrderBy(on => on.UserTypeName).ToList();
                        break;
                    case "CreatedDate":
                        user = user.OrderBy(on => on.CreatedDate).ToList();
                        break;
                    case "UpdatedDate":
                        user = user.OrderBy(on => on.UpdatedDate).ToList();
                        break;
                    case "Active":
                        user = user.OrderBy(on => on.Active).ToList();
                        break;
                    case "TypeUser":
                        user = user.OrderBy(on => on.TypeUser).ToList();
                        break;
                    default:
                        user = user.OrderBy(on => on.CreatedDate).ToList();
                        break;
                }
                user = user.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                    .Take(pageParams.PageSize).ToList();
            }
            return user;
        }

        //checking existUser
        public bool checkExistUser(TbUser tbUser)
        {
            var email = contractorFindingContext.TbUsers.Where(e => e.EmailId == tbUser.EmailId).FirstOrDefault();
            return email == null;
        }

        //for Registration
        public bool Register(Registration registration)
        {
            bool check = checkExistUser(registration);
            if (check == true)
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
                    return true;
                }
                return false;
            }
            return false;
        }
            
        ////for Login
        public Tuple<string,int> Login(TbUser login)
        {
            string checkingpassword = encrypt.EncodePasswordToBase64(login.Password);

            var myUser = contractorFindingContext.TbUsers.
                FirstOrDefault(u => u.EmailId == login.EmailId
                && u.Password == checkingpassword);
            if (myUser != null)
            {
                login.TypeUser = myUser.TypeUser;
                var token = generateToken.GenerateNewToken(login);
                Tuple<string, int> myid = new Tuple<string, int>(token, myUser.UserId);
                return myid;
            }
            return null!;
        }

        //for forgotpassword case
        public bool forgotpassword(Registration login)
        {

            var userWithSameEmail = contractorFindingContext.TbUsers.Where(m => m.EmailId == login.EmailId).SingleOrDefault();
            if (userWithSameEmail != null)
            {
                string encrptnewpassword = encrypt.EncodePasswordToBase64(login.Password);
                string encrptconfirmpassword = encrypt.EncodePasswordToBase64(login.confirmationPassword);
                if (encrptnewpassword == encrptconfirmpassword)
                {
                    userWithSameEmail.Password = encrptconfirmpassword;
                    userWithSameEmail.UpdatedDate = DateTime.Now;
                    contractorFindingContext.Entry(userWithSameEmail).State = EntityState.Modified;
                    contractorFindingContext.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;            
        }

        //for delete deatils
        public bool DeleteUser(TbUser user)
        {
            var checkid = contractorFindingContext.TbUsers.Where(x => x.UserId == user.UserId).FirstOrDefault();
            if (checkid != null)
            {
                contractorFindingContext.TbUsers.Remove(user);
                contractorFindingContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Userview> GetUsers()
        {
            List<Userview> users = contractorFindingContext.Userviews.ToList();
            return users;
        }
    }
}
