using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Test.Orders;

var builder = FunctionsApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("OrderReceivedFunction");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
transport.ConnectionString(Environment.GetEnvironmentVariable("SERVICEBUSCONNSTR_SERVICEBUS"));
transport.Routing().RouteToEndpoint(typeof(Order), "FundServicesQueue");

endpointConfiguration.SendOnly();

var endpointInstance = await Endpoint.Start(endpointConfiguration);
builder.Services.AddSingleton<IMessageSession>(endpointInstance);

builder.Services.AddSingleton(endpointConfiguration);
builder.ConfigureFunctionsWebApplication();
builder.Services.AddApplicationInsightsTelemetryWorkerService()
                .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
