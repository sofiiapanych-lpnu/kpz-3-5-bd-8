using System;
using System.Collections.Generic;

namespace lab3_5.Server.Models;

public partial class Client
{
    public int PersonId { get; set; }

    public int? AddressId { get; set; }

    public virtual DeliveryAddress? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Person Person { get; set; } = null!;
}
