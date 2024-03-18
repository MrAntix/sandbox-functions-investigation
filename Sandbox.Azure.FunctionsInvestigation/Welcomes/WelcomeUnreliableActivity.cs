using Microsoft.DurableTask;
using Sandbox.Azure.FunctionsInvestigation.RandomFailures;

namespace Sandbox.Azure.FunctionsInvestigation.Welcomes;

public class WelcomeUnreliableActivity(
    IRandomFailureService service
    ) : TaskActivity<string, string>
{
    public override async Task<string> RunAsync(
        TaskActivityContext context,
        string input
        )
    {
        if (await service.ExecuteAsync()) return "Welcome!";

        throw new Exception("Oh No");
    }
}
