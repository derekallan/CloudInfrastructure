namespace ServiceBusConsumer;

public static class MetricsTracker
{
    public static int OrderCount { get; set; }
    public static List<double> Delays { get; set; } = new List<double>(1000);
    
    public static void NewOrder()
    {
        OrderCount++;
    }

    public static void NewDelayMs(double delay)
    {
        Delays.Add(delay);
    }
}
