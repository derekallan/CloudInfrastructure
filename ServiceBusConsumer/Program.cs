// See https://aka.ms/new-console-template for more information
using ServiceBusConsumer;

Console.WriteLine("Hello, World!");

var endpointConfiguration = new EndpointConfiguration("FundServicesQueue");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();
endpointConfiguration.Conventions()
                    .DefiningMessagesAs(t => t.UnderlyingSystemType.IsAssignableTo(typeof(Contracts.IMessage)))
                    .DefiningEventsAs(t => t.UnderlyingSystemType.IsAssignableTo(typeof(Contracts.IEvent)));

var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
transport.ConnectionString(Environment.GetEnvironmentVariable("SERVICEBUSCONNSTR_SERVICEBUS"));
transport.TopicName("fundservicestopic");

var endpointInstance = await Endpoint.Start(endpointConfiguration);

var cancel = new CancellationTokenSource();
var metricsLogger = new MetricsLogger();
var loggingTask = metricsLogger.LogOrderThroughput(cancel.Token);
var loggingTask2 = metricsLogger.LogAverageDelay(cancel.Token);

Console.ReadLine();
cancel.Cancel();
await Task.WhenAll(loggingTask, loggingTask2);
Console.WriteLine("Done");
