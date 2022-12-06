﻿using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserService
    {
        public List<UserDisplay> GetUserDetails();
        bool checkExistUser(TbUser tbUser);
        bool Register(Registration registration);
 
        string Login(TbUser login);
       
        bool forgotpassword(Registration login);
        public bool DeleteUser(TbUser user);

    }
}
