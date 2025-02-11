﻿// See https://aka.ms/new-console-template for more information
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;
using PublishEvent;

var config = new ConfigurationBuilder()
.AddUserSecrets<Program>()
.AddJsonFile("appsettings.json", optional: true)
.Build();

var client = new EventGridPublisherClient(new Uri(config["EventGridTopic"]), new AzureKeyCredential(config["EventGridKey"]));

var events = new List<EventGridEvent>
{
    new EventGridEvent(
        "ExampleEventSubject",
        "Example.EventType",
        "1.0",
        new Order
        {
            OrderId = "123",
            OrderDate = DateTime.UtcNow,
            OrderAmount = 100
        }
    )
};

await client.SendEventsAsync(events);

Console.WriteLine("Event Published");