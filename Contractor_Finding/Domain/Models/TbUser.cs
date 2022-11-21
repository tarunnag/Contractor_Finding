using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbUser
{
    public int UserId { get; set; }

    public int? TypeUser { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string EmailId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long PhoneNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<ContractorDetail> ContractorDetails { get; } = new List<ContractorDetail>();

    public virtual UserType? TypeUserNavigation { get; set; }
}
