using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Sandbox.Azure.FunctionsInvestigation.Welcomes;

public class WelcomeFunction(
    ILogger<WelcomeFunction> logger
    )
{
    [Function(nameof(WelcomeFunction))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "get",
            Route = ROUTES.WELCOME
            )] HttpRequestData req,
        [DurableClient] DurableTaskClient client
        )
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var startOptions = new StartOrchestrationOptions(
            InstanceId: $"ID_{Guid.NewGuid():N}",
            StartAt: DateTimeOffset.Now.AddMinutes(1)
            );

        var instanceId = await client
            .ScheduleNewWelcomeOrchestratorInstanceAsync(
                   req.Method,
                   startOptions
                   );

        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }
}