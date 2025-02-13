using Contracts;

namespace ServiceBusConsumer;

public class OrderMessageHandler : IHandleMessages<Order>
{
    public Task Handle(Order message, IMessageHandlerContext context)
    {
        MetricsTracker.NewOrder();
        var delay = DateTime.UtcNow - message.OrderDate;
        MetricsTracker.NewDelayMs(delay.TotalMilliseconds);
        return Task.CompletedTask;
    }
}
