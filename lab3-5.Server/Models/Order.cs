using System;
using System.Collections.Generic;

namespace lab3_5.Server.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public decimal Cost { get; set; }

    public string Description { get; set; } = null!;

    public int? ClientId { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}
