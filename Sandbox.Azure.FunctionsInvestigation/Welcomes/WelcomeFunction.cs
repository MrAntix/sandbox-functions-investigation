using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Sandbox.Azure.FunctionsInvestigation.Welcomes;

public class WelcomeFunction(
    ILogger<WelcomeFunction> logger
    )
{
    [Function(nameof(WelcomeFunction))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client
        )
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                   nameof(WelcomeOrchestrator), req.Method, CancellationToken.None
                   );

        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }
}