using System;
using System.Collections.Generic;

namespace Domain;

public partial class Userview
{
    public int UserId { get; set; }

    public string? Usertype { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string EmailId { get; set; } = null!;

    public long PhoneNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }
}
