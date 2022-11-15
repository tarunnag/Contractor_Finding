using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    internal interface IUserService
    {
        List<UserDetail> GetUserDetails();
        bool Create(UserDetail userDetail);
        bool DeleteUser(UserDetail userDetail);


    }
}
