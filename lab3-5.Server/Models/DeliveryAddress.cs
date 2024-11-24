using System;
using System.Collections.Generic;

namespace lab3_5.Server.Models;

public partial class DeliveryAddress
{
    public int DeliveryAddressId { get; set; }

    public string Street { get; set; } = null!;

    public string? BuildingNumber { get; set; }

    public string? ApartmentNumber { get; set; }

    public int? Floor { get; set; }

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}
