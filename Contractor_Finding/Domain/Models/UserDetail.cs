using System;
using System.Collections.Generic;

namespace Domain;

public partial class UserDetail
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? TypeUser { get; set; }

    public int? Gender { get; set; }

    public string MailId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long PhoneNumber { get; set; }

    public int? StateName { get; set; }

    public int? CityName { get; set; }

    public virtual CityName? CityNameNavigation { get; set; }

    public virtual ICollection<ContractorDetail> ContractorDetails { get; } = new List<ContractorDetail>();

    public virtual Gender? GenderNavigation { get; set; }

    public virtual StateName? StateNameNavigation { get; set; }

    public virtual TypeUser? TypeUserNavigation { get; set; }
}
