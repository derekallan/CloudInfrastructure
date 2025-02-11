using System;

namespace PublishEvent;

public class Order
{
    public string OrderId { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public decimal OrderAmount { get; set; }
}
