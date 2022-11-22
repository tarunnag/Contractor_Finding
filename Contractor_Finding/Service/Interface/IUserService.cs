using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserService
    {
        public List<UserDisplay> GetUserDetails();
        bool checkExistUser(TbUser tbUser);
        bool Register(Registration registration);
        //string Register(Registration registration);
        //string Login(Login login);
        bool Login(Login login);
        //string forgotpassword(Login login);
        bool forgotpassword(Login login);
        bool DeleteUser(TbUser user);
    }
}
