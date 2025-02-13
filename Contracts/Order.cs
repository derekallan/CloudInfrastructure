using System;

namespace Contracts;

public class Order : IEvent
{
    public string OrderId { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public decimal OrderAmount { get; set; }
}
