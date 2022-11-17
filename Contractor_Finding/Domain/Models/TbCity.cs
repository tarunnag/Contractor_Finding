using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbCity
{
    public int? StateId { get; set; }

    public int CityId { get; set; }

    public string? CityName { get; set; }

    public virtual TbState? State { get; set; }
}
