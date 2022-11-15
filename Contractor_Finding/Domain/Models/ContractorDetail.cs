using System;
using System.Collections.Generic;

namespace Domain;

public partial class ContractorDetail
{
    public int? ContractorId { get; set; }

    public string? CompanyName { get; set; }

    public string License { get; set; } = null!;

    public int? Services { get; set; }

    public double? Location { get; set; }

    public virtual UserDetail? Contractor { get; set; }

    public virtual Map? LocationNavigation { get; set; }

    public virtual ServiceProviding? ServicesNavigation { get; set; }
}
