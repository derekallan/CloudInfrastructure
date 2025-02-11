// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Test.Orders;

public class Order
{
    public string OrderId { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public decimal OrderAmount { get; set; }
}

public class OrderReceivedTrigger
{
    private readonly ILogger<OrderReceivedTrigger> _logger;

    public OrderReceivedTrigger(ILogger<OrderReceivedTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(OrderReceivedTrigger))]
    public void Run([EventGridTrigger] EventGridEvent eventGridEvent)
    {
        try
        {
            _logger.LogInformation("Event type: {type}, Event subject: {subject}", eventGridEvent.EventType, eventGridEvent.Subject);
            _logger.LogInformation("Data: {Data}", eventGridEvent.Data);

            if (eventGridEvent.Data is not null)
            {
                var order = JsonSerializer.Deserialize<Order>(eventGridEvent.Data.ToString(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (order is null)
                {
                    _logger.LogWarning("Failed to deserialize the order data.");
                    return;
                }
                _logger.LogInformation("Order received: {orderId}, {orderDate}, {orderAmount}", order.OrderId, order.OrderDate, order.OrderAmount);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred processing the order.");
        }
    }
}
