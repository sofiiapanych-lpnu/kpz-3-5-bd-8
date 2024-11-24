using System;
using System.Collections.Generic;

namespace lab3_5.Server.Models;

public partial class Courier
{
    public int PersonId { get; set; }

    public string? LicencePlate { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual Transport? LicencePlateNavigation { get; set; }

    public virtual Person Person { get; set; } = null!;
}
