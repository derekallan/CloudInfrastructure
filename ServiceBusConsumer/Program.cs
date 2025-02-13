// See https://aka.ms/new-console-template for more information
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

Console.ReadLine();
