namespace ServiceBusConsumer;

public class MetricsLogger
{
    public async Task LogOrderThroughput(CancellationToken cancellationToken)
    {
        var lastOrderCount = MetricsTracker.OrderCount;
        while (!cancellationToken.IsCancellationRequested)
        {
            var currentOrderCount = MetricsTracker.OrderCount;
            var ordersPerSecond = currentOrderCount - lastOrderCount;
            Console.WriteLine($"Orders per second: {ordersPerSecond}");
            Console.ResetColor();
            lastOrderCount = currentOrderCount;   
            await Task.Delay(1000, cancellationToken);
        }
        return;
    }
    public async Task LogAverageDelay(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (MetricsTracker.Delays.Count == 0)
            {
                await Task.Delay(1000, cancellationToken);
                continue;
            }
            var average = MetricsTracker.Delays.Average();
            MetricsTracker.Delays.Clear();
            Console.WriteLine($"Average Delay For Orders Received In Last Second: {average}ms");
            Console.ResetColor();
            await Task.Delay(1000, cancellationToken);
        }
        return;
    }
}