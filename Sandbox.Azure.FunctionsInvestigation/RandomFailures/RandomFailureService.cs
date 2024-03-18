namespace Sandbox.Azure.FunctionsInvestigation.RandomFailures;

public class RandomFailureService : IRandomFailureService
{
    readonly Random _random = new();

    async Task<bool> IRandomFailureService.ExecuteAsync()
    {
        await Task.Delay(1000);

        return _random.Next(1) != 0;
    }
}
