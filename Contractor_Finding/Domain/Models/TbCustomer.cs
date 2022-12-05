using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbCustomer
{
    public double LandSqft { get; set; }

    public string RegistrationNo { get; set; } = null!;

    public int? BuildingType { get; set; }

    public double? Lattitude { get; set; }

    public double? Longitude { get; set; }

    public int Pincode { get; set; }

    public int? CustomerId { get; set; }

    public virtual TbBuilding? BuildingTypeNavigation { get; set; }

    public virtual TbUser? Customer { get; set; }
}
