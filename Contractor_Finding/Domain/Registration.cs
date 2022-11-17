using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Registration : TbUser
    {
        public string? confirmationPassword { get; set; }
    }
}
