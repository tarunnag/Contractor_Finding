using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public  class ContractorDisplay :ContractorDetail
    {
        public string? Services { get; set; }
        public string? Gender { get; set; }
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string EmailId { get; set; } = null!;
    }
}
