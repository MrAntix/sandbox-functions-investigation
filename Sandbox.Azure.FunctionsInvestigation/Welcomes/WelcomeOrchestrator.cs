using Microsoft.DurableTask;

namespace Sandbox.Azure.FunctionsInvestigation.Welcomes;

[DurableTask(nameof(WelcomeOrchestrator))]
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

        welcomes.Add(await context.CallWelcomeUnreliableActivityAsync("One", options));

        var tasks = new Task<string>[] {
            context.CallWelcomeUnreliableActivityAsync("Two", options),
            context.CallWelcomeUnreliableActivityAsync("Three", options)
        };

        await Task.WhenAll(tasks);
        welcomes.AddRange(tasks.Select(t => t.Result));


        return string.Join(" ", welcomes);
    }
}
