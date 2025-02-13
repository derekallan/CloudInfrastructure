using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("OrderReceivedFunction");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.Conventions()
    .DefiningMessagesAs(t => t.UnderlyingSystemType.IsAssignableTo(typeof(Contracts.IMessage)))
    .DefiningEventsAs(t => t.UnderlyingSystemType.IsAssignableTo(typeof(Contracts.IEvent)));
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
transport.ConnectionString(Environment.GetEnvironmentVariable("SERVICEBUSCONNSTR_SERVICEBUS"));
transport.TopicName("fundservicestopic");

endpointConfiguration.SendOnly();

var endpointInstance = await Endpoint.Start(endpointConfiguration);
builder.Services.AddSingleton<IMessageSession>(endpointInstance);

builder.Services.AddSingleton(endpointConfiguration);
builder.ConfigureFunctionsWebApplication();
builder.Services.AddApplicationInsightsTelemetryWorkerService()
                .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
