using Contracts;

namespace ServiceBusConsumer;

public class OrderMessageHandler : IHandleMessages<Order>
{
    public Task Handle(Order message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Order received: {message.OrderId}, {message.OrderDate}, {message.OrderAmount}");
        return Task.CompletedTask;
    }
}
