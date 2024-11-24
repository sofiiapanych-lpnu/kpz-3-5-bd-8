using System;
using System.Collections.Generic;

namespace lab3_5.Server.Models;

public partial class CourierPerformance
{
    public int? PersonId { get; set; }

    public string? CourierName { get; set; }

    public long? TotalDeliveries { get; set; }

    public TimeSpan? AvgDeliveryTime { get; set; }

    public long? DeliveryRank { get; set; }
}
