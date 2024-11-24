using System;
using System.Collections.Generic;

namespace lab3_5.Server.Models;

public partial class Transport
{
    public string LicencePlate { get; set; } = null!;

    public string TransportType { get; set; } = null!;

    public string Model { get; set; } = null!;

    public virtual Courier? Courier { get; set; }
}
