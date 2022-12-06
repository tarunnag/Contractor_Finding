using Domain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IGenerateToken
    {
        string GenerateNewToken(TbUser user);
        string? ValidateJwtToken(string token);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);

    }
}
