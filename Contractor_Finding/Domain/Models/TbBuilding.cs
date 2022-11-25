using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbBuilding
{
    public int Id { get; set; }

    public string? Building { get; set; }

    public virtual ICollection<TbCustomer> TbCustomers { get; } = new List<TbCustomer>();
}
