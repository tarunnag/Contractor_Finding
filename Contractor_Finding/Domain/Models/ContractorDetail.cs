using System;
using System.Collections.Generic;

namespace Domain;

public partial class ContractorDetail
{
    public int? ContractorId { get; set; }

    public string? CompanyName { get; set; }

    public int? Gender { get; set; }

    public string? License { get; set; }

    public int? Services { get; set; }

    public double? Place { get; set; }

    public int Pincode { get; set; }

    public virtual UserType? Contractor { get; set; }

    public virtual TbGender? GenderNavigation { get; set; }

    public virtual Map? PlaceNavigation { get; set; }

    public virtual ServiceProviding? ServicesNavigation { get; set; }
}
