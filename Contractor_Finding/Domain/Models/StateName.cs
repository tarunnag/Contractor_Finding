using System;
using System.Collections.Generic;

namespace Domain;

public partial class StateName
{
    public int StateId { get; set; }

    public string StateName1 { get; set; } = null!;

    public virtual ICollection<UserDetail> UserDetails { get; } = new List<UserDetail>();
}
