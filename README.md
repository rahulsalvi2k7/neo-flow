# neo-flow

Extensible, lightweight workflow engine for .NET 8

Neo-flow provides a small, composable set of primitives to build business workflows in C#. It focuses on clarity, async-first execution, and easy extension points for custom steps, conditions, and observers.

## Features

- Composable builders and step types: sequential, conditional, switch, parallel, and conditional-parallel
- Async-first execution with `CancellationToken` support
- Extensible interfaces for custom steps, conditions, and observers
- Unit tests covering core step behaviors (see tests under `tests/`)

## Quick start

### Prerequisites
**Note:** Neo-flow targets .NET 8.0. Install the .NET 8 SDK or later.

- Install .NET SDK: https://dotnet.microsoft.com/

### Build the solution

Build the full solution from the repository root:

```bash
dotnet build src/neo-flow/neo-flow.sln
```

### Run the sample app

Run the minimal console host from the solution folder:

```bash
dotnet run --project src/neo-flow/neo.flow.app/neo.flow.app.csproj
```

### Run tests

Run all tests or individual test projects:

```bash
dotnet test tests/neo.flow.core.tests/neo.flow.core.tests.csproj
dotnet test tests/neo.flow.logger.console.tests/neo.flow.logger.console.tests.csproj
```

## Key concepts

- `IBusinessStep`: unit of work executed by the engine
- `IExecutionContext`: carries state across steps during execution
- `ICondition`: predicate used by conditional and switch steps
- `IStepObserver`: receives lifecycle events for observability and logging

The repository provides implementations for common composition patterns under `src/neo-flow/neo.flow.core/Steps` and fluent helpers in `src/neo-flow/neo.flow.core/Builder`.

## Example (conceptual)

```csharp
var workflow = new WorkflowBuilder("Example")
    .Step(new LogStep("Start"))
    .Step(new ConditionalStep(myCondition, thenStep, elseStep))
    .Build();

await engine.RunAsync(workflow, new ExecutionContext(), CancellationToken.None);
```

## Projects

- `src/neo-flow/neo.flow.core` — core library with builders, steps, engine, and models
- `src/neo-flow/neo.flow.app` — minimal host application demonstrating usage
- `src/neo-flow/neo.flow.logger.console` — console logger helpers

## Tests

Unit tests are under the `tests/` folder. Run test projects with `dotnet test` as shown above.

## Next steps

- Explore `src/neo-flow/neo.flow.core/Builder` to compose workflows with the fluent API
- Implement your domain-specific `IBusinessStep` and `ICondition`
- Hook an `IStepObserver` to capture execution events for telemetry or auditing

[//]: # (License and contribution guidelines are kept in their respective files.)
