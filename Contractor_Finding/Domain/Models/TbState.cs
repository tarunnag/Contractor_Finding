using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbState
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public virtual ICollection<TbCity> TbCities { get; } = new List<TbCity>();
}
