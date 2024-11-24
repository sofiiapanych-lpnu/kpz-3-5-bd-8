using System;
using System.Collections.Generic;

namespace lab3_5.Server.Models;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public int? OrderId { get; set; }

    public int? CourierId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public TimeSpan? DesiredDuration { get; set; }

    public TimeSpan? ActualDuration { get; set; }

    public int? WarehouseId { get; set; }

    public int? AddressId { get; set; }

    public string? Status { get; set; }

    public virtual DeliveryAddress? Address { get; set; }

    public virtual Courier? Courier { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
