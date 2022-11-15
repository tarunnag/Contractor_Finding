using System;
using System.Collections.Generic;

namespace Domain;

public partial class TypeUser
{
    public int? Id { get; set; }

    public string UserType { get; set; } = null!;

    public DateTime? Duration { get; set; }

    public virtual ICollection<UserDetail> UserDetails { get; } = new List<UserDetail>();
}
