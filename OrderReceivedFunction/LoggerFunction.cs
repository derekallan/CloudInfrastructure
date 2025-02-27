// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Test;

public class LoggerFunction
{
    private readonly ILogger<LoggerFunction> _logger;

    public LoggerFunction(ILogger<LoggerFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(LoggerFunction))]
    public void Run([EventGridTrigger] EventGridEvent eventGridEvent)
    {
        _logger.LogInformation("Event type: {type}, Event Data: {Data}", eventGridEvent.EventType, eventGridEvent.Data);
    }
}
