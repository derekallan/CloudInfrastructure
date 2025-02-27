// using System.Text.Json;
// using Azure.Messaging.EventGrid;
// using Contracts;
// using Microsoft.Azure.Functions.Worker;
// using Microsoft.Extensions.Logging;

// namespace Test.Orders;

// public class OrderReceivedTrigger
// {
//     private readonly IMessageSession _messageSession;
//     private readonly ILogger<OrderReceivedTrigger> _logger;

//     public OrderReceivedTrigger(
//         IMessageSession messageSession,
//         ILogger<OrderReceivedTrigger> logger)
//     {
//         _messageSession = messageSession;
//         _logger = logger;
//     }

//     [Function(nameof(OrderReceivedTrigger))]
//     public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
//     {
//         try
//         {
//             _logger.LogInformation("Event type: {type}, Event subject: {subject}", eventGridEvent.EventType, eventGridEvent.Subject);
//             _logger.LogInformation("Data: {Data}", eventGridEvent.Data);

//             if (eventGridEvent.Data is not null)
//             {
//                 var order = JsonSerializer.Deserialize<Order>(eventGridEvent.Data.ToString(), new JsonSerializerOptions
//                 {
//                     PropertyNameCaseInsensitive = true
//                 });
//                 if (order is null)
//                 {
//                     _logger.LogWarning("Failed to deserialize the order data.");
//                     return;
//                 }
//                 _logger.LogInformation("Order received: {orderId}, {orderDate}, {orderAmount}", order.OrderId, order.OrderDate, order.OrderAmount);
//                 await _messageSession.Publish(order);
//             }
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "An error occurred processing the order.");
//         }
//     }
// }
