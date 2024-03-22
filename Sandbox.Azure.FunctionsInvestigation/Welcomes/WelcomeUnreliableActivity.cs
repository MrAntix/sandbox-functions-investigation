using Microsoft.DurableTask;
using Sandbox.Azure.FunctionsInvestigation.RandomFailures;

namespace Sandbox.Azure.FunctionsInvestigation.Welcomes;

[DurableTask(nameof(WelcomeUnreliableActivity))]
public class WelcomeUnreliableActivity(
    IRandomFailureService service
    ) : TaskActivity<string, string>
{
    public override async Task<string> RunAsync(
        TaskActivityContext context,
        string input
        )
    {
        if (await service.ExecuteAsync()) return $"Welcome {input}!";

        throw new Exception("Oh No");
    }
}
