using Microsoft.DurableTask;

namespace Sandbox.Azure.FunctionsInvestigation.Welcomes;

public class WelcomeOrchestrator : TaskOrchestrator<string, string>
{
    public override async Task<string> RunAsync(
        TaskOrchestrationContext context,
        string input
        )
    {
        var welcomes = new List<string>();

        var options = new TaskOptions(
            new TaskRetryOptions(
                new RetryPolicy(5, TimeSpan.FromSeconds(1)))
            );

        welcomes.Add(await context.CallActivityAsync<string>(nameof(WelcomeUnreliableActivity), options));

        var tasks = new Task<string>[] {
            context.CallActivityAsync<string>(nameof(WelcomeUnreliableActivity), options),
            context.CallActivityAsync<string>(nameof(WelcomeUnreliableActivity), options)
        };

        await Task.WhenAll(tasks);
        welcomes.AddRange(tasks.Select(t => t.Result));


        return string.Join(" ", welcomes);
    }
}
