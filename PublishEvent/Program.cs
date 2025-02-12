// See https://aka.ms/new-console-template for more information
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;
using PublishEvent;

var config = new ConfigurationBuilder()
.AddUserSecrets<Program>()
.AddJsonFile("appsettings.json", optional: true)
.Build();

var client = new EventGridPublisherClient(new Uri(config["EventGridTopic"]), new AzureKeyCredential(config["EventGridKey"]));

var tasks = new List<Task>();
var messagesToSend = 10;
for (var i = 0; i < messagesToSend; i++)
{
    var order = new Order
    {
        OrderId = Guid.NewGuid().ToString(),
        OrderDate = DateTime.UtcNow,
        OrderAmount = 100
    };

    var events = new List<EventGridEvent>
    {
        new EventGridEvent(
            "ExampleEventSubject",
            "Example.EventType",
            "1.0",
            order
        )
    };

    tasks.Add(client.SendEventsAsync(events));
}

await Task.WhenAll(tasks);
Console.WriteLine($"{messagesToSend} Event(s) Published");