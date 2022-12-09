using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserDisplayV2
    {
        public int UserId { get; set; }

        public string? Usertype { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string EmailId { get; set; } = null!;

        public long PhoneNumber { get; set; }
    }
}
