using System;
using System.Collections.Generic;

namespace Domain;

public partial class ServiceProviding
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public virtual ICollection<ContractorDetail> ContractorDetails { get; } = new List<ContractorDetail>();
}
