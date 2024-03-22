using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sandbox.Azure.FunctionsInvestigation.RandomFailures;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IRandomFailureService, RandomFailureService>();
    })
    .Build();

host.Run();