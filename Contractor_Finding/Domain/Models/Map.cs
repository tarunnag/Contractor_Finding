using System;
using System.Collections.Generic;

namespace Domain;

public partial class Map
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double PlaceId { get; set; }

    public virtual ICollection<ContractorDetail> ContractorDetails { get; } = new List<ContractorDetail>();
}
