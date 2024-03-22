using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Sandbox.Azure.FunctionsInvestigation.Tests;

public class FunctionsTests : IDisposable
{
    const int PORT = 7020;
    readonly IDisposable _functions;
    readonly HttpClient _http;

    public FunctionsTests(ITestOutputHelper output)
    {
        _functions = FunctionsHelper.StartHost(PORT, output);
        _http = new HttpClient();
        _http.BaseAddress = new Uri($"http://localhost:{PORT}/api/");
    }

    [Fact]
    public async Task TriggerWelcomeAsync()
    {
        var functionResponse = await _http.GetAsync(
            ROUTES.WELCOME
            );
        Assert.Equal(HttpStatusCode.Accepted, functionResponse.StatusCode);

        var functionResponseData = await functionResponse
                .Content.ReadFromJsonAsync<FunctionResponseData>();
        Assert.NotNull(functionResponseData?.statusQueryGetUri);

        StatusResponseData? statusResponseData;
        do
        {
            await Task.Delay(50);
            var statusResponse = await _http.GetAsync(
                functionResponseData.statusQueryGetUri
                );
            Assert.Equal(HttpStatusCode.Accepted, functionResponse.StatusCode);

            statusResponseData = await statusResponse
                .Content.ReadFromJsonAsync<StatusResponseData>();

        } while (statusResponseData is not null
            && statusResponseData.runtimeStatus is "Running" or "Pending");

        Assert.Contains(statusResponseData?.runtimeStatus, ["Failed", "Completed"]);
    }

    public void Dispose()
    {
        _http.Dispose();
        _functions.Dispose();
    }

    sealed record FunctionResponseData(string statusQueryGetUri);
    sealed record StatusResponseData(string runtimeStatus);
}
