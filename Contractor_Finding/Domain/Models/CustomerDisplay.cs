using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CustomerDisplay
    {       
            public double? LandSqft { get; set; }

            public string RegistrationNo { get; set; } = null!;

            public string? BuildingType { get; set; }

            public double? Lattitude { get; set; }

            public double? Longitude { get; set; }

            public int? Pincode { get; set; }
       
    }
}
