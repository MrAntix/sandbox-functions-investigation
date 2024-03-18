namespace Sandbox.Azure.FunctionsInvestigation.RandomFailures;

public interface IRandomFailureService
{
    Task<bool> ExecuteAsync();
}
