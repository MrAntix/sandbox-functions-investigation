# sandbox-functions-investigation

trying to get durable orchestrations working in Azure Functions

---------------------------------------------------------------

This app is a basic demo or an orchestrator which is triggered by an HTTP GET and call an unreliable service (IRandomFailureService) in serial and paralell tasks.

It shows:

- How to use class-based Orchestrator and Activity tasks
- How you can retry activities to mitigate services which may occasionally fail, and retry causes no side effects.
- Which packages you need and work at the time of writing (see .csproj files)
- A nieve test harness to call your function from
