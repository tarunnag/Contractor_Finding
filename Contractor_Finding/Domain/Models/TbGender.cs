using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbGender
{
    public int GenderId { get; set; }

    public string? GenderType { get; set; }

    public virtual ICollection<ContractorDetail> ContractorDetails { get; } = new List<ContractorDetail>();
}
