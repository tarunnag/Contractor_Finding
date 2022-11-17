using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserDisplay
    {
        public int UserId { get; set; }

        public string? TypeUser { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string EmailId { get; set; } = null!;

        public long PhoneNumber { get; set; }
    }
}
