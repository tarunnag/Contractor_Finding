using Domain;
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
        string checkExistUser(TbUser tbUser);
        string Register(Registration registration);
 
        string Login(Login login);
       
        string GenerateToken(TbUser user);
        string forgotpassword(Login login);
        bool DeleteUser(TbUser user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
        string? ValidateJwtToken(string token);
    }
}
