using System;

namespace Contracts;

public class Order : IEvent
{
    public string OrderId { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public decimal OrderAmount { get; set; }
    public string CustomerId { get; set; } = string.Empty;

    public string ProductId { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string ShippingAddress { get; set; } = string.Empty;

    public string BillingAddress { get; set; } = string.Empty;

    public string OrderStatus { get; set; } = string.Empty;

    public DateTime ShippingDate { get; set; }

    public DateTime DeliveryDate { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public decimal ShippingCost { get; set; }

    public decimal TaxAmount { get; set; }

    public string Currency { get; set; } = string.Empty;
}
