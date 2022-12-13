using System;
using System.Collections.Generic;

namespace Domain;

public partial class CustomerView
{
    public int UserId { get; set; }

    public string RegistrationNo { get; set; } = null!;

    public string? Building { get; set; }

    public double LandSqft { get; set; }

    public double? Lattitude { get; set; }

    public double? Longitude { get; set; }

    public int Pincode { get; set; }
}
