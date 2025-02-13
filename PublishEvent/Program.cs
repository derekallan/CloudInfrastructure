// See https://aka.ms/new-console-template for more information
using Azure;
using Azure.Messaging.EventGrid;
using Contracts;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
.AddUserSecrets<Program>()
.AddJsonFile("appsettings.json", optional: true)
.Build();

var client = new EventGridPublisherClient(new Uri(config["EventGridTopic"]), new AzureKeyCredential(config["EventGridKey"]));

var cancel = new CancellationTokenSource();

var backgroundTask = Task.Factory.StartNew(async () =>
{
    var orderId = 1;
    var ordersPerBatch = 10;
    while (!cancel.Token.IsCancellationRequested)
    {
        var events = new List<EventGridEvent>();
        for (int j = 0; j < ordersPerBatch; j++)
        {
            events.Add(new EventGridEvent(
                    "ExampleEventSubject",
                    "Example.EventType",
                    "1.0",
                    new Order
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        OrderDate = DateTime.UtcNow,
                        OrderAmount = orderId++,
                    }
                ));
        }
        await Task.WhenAll(client.SendEventsAsync(events), Task.Delay(100));
        Console.WriteLine($"{ordersPerBatch} Order(s) Published");
    }
}, cancel.Token);


// var tasks = new List<Task>();
// var batches = 10;
// var ordersPerBatch = 1;
// var orderNumber = 1;
// for (var i = 0; i < batches; i++)
// {
//     var events = new List<EventGridEvent>();
//     for (int j = 0; j < ordersPerBatch; j++)
//     {
//         var order = new Order
//         {
//             OrderId = Guid.NewGuid().ToString(),
//             OrderDate = DateTime.UtcNow,
//             OrderAmount = orderNumber++,
//         };

//         events.Add(new EventGridEvent(
//             "ExampleEventSubject",
//             "Example.EventType",
//             "1.0",
//             order
//         ));
//     }

//     tasks.Add(client.SendEventsAsync(events));
// }


while (true)
{
    Console.WriteLine("Press 'q' to quit");
    var key = Console.ReadKey();
    if (key.KeyChar == 'q')
    {
        cancel.Cancel();
        break;
    }
}

await backgroundTask;

Console.WriteLine("Done");
Console.Write("Press any key to exit");
Console.ReadKey();