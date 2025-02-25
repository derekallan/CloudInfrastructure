using System.Collections.Concurrent;

namespace ServiceBusConsumer;

public static class MetricsTracker
{
    public static int OrderCount { get; set; }
    public static ConcurrentBag<double> Delays { get; set; } = new ConcurrentBag<double>();

    public static void NewOrder()
    {
        OrderCount++;
    }

    public static void NewDelayMs(double delay)
    {
        Delays.Add(delay);
    }
}
