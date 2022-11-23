using System;
using System.Collections.Generic;

namespace Domain;

public partial class ContractorDetail
{
    public int? ContractorId { get; set; }

    public string CompanyName { get; set; } = null!;

    public int? Gender { get; set; }

    public string License { get; set; } = null!;

    public int? Services { get; set; }

    public double? Lattitude { get; set; }

    public double? Longitude { get; set; }

    public int Pincode { get; set; }

    public long? PhoneNumber { get; set; }

    public virtual TbUser? Contractor { get; set; }

    public virtual TbGender? GenderNavigation { get; set; }

    public virtual ServiceProviding? ServicesNavigation { get; set; }
}
