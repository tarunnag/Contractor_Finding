using System;
using System.Collections.Generic;

namespace Domain;

public partial class UserType
{
    public int TypeId { get; set; }

    public string? Usertype1 { get; set; }

    public virtual ICollection<TbUser> TbUsers { get; } = new List<TbUser>();
}
