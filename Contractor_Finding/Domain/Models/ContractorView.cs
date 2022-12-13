using System;
using System.Collections.Generic;

namespace Domain;

public partial class ContractorView
{
    public int UserId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? GenderType { get; set; }

    public string License { get; set; } = null!;

    public string? ServiceName { get; set; }

    public double? Lattitude { get; set; }

    public double? Longitude { get; set; }

    public long? PhoneNumber { get; set; }

    public int Pincode { get; set; }
}
