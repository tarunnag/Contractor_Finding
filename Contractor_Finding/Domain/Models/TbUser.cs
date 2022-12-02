using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbUser
{
    public int UserId { get; set; }

    public int TypeUser { get; set; }

    public string FirstName { get; set; } 

    public string? LastName { get; set; }

    public string EmailId { get; set; } 

    public string Password { get; set; } 

    public long PhoneNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<ContractorDetail> ContractorDetails { get; } = new List<ContractorDetail>();
    public virtual ICollection<TbCustomer> Customers { get; } = new List<TbCustomer>();

    public virtual UserType? TypeUserNavigation { get; set; }
}
