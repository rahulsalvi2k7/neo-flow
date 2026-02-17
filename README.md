# neo-flow

Extensible, lightweight workflow engine for .NET 8

Neo-flow provides a small, composable set of primitives to build business workflows in C#. It focuses on clarity, async-first execution, and easy extension points for custom steps, conditions, and observers.

## Features

- Composable building blocks: sequential, conditional, switch, parallel, and conditional-parallel steps
- Async-first execution with CancellationToken support
- Extensible interfaces for custom steps and observers
- Test coverage for core step behaviors (see `tests/neo.flow.core.tests`)

## Quick start

### Prerequisites
> **Note:** Neo-flow targets .NET 8.0. Use the SDK for .NET 8 or later.

- Install .NET SDK: https://dotnet.microsoft.com/

### Build and run the sample app

```bash
dotnet build
dotnet run --project src/neo-flow/neo.flow.app/neo.flow.app.csproj
```

### Run tests

```bash
dotnet test tests/neo.flow.core.tests
dotnet test tests/neo.flow.logger.console.tests
```

## Key concepts

- `IBusinessStep`: unit of work executed by the engine.
- `IExecutionContext`: carries state across steps during execution.
- `ICondition`: predicate used by conditional and switch steps.
- `IStepObserver`: receive lifecycle events for observability and logging.

Neo-flow ships with implementations for common composition patterns (sequential, parallel, conditional, switch, conditional-parallel) under `src/neo-flow/neo.flow.core/Steps` and fluent helpers in `src/neo-flow/neo.flow.core/Builder`.

## Example (conceptual)

```csharp
var workflow = new WorkflowBuilder("Example")
    .Step(new LogStep("Start"))
    .Step(new ConditionalStep(myCondition, thenStep, elseStep))
    .Build();

await engine.RunAsync(workflow, new ExecutionContext(), CancellationToken.None);
```

The projects under `src/neo-flow` contain core libraries and a small console app demonstrating usage:

- `src/neo-flow/neo.flow.core` — core library with builders, steps, engine, and models
- `src/neo-flow/neo.flow.app` — minimal host application showing how to compose and run workflows
- `src/neo-flow/neo.flow.logger.console` — console logger helpers for development and diagnostics

## Tests

Unit tests live in the `tests` folder. Run them with `dotnet test` as shown above. The test projects exercise the various step types and logging components.

## Next steps

- Explore `src/neo-flow/neo.flow.core/Builder` to compose workflows with the fluent API.
- Implement `IBusinessStep` and `ICondition` for your domain logic.
- Hook an `IStepObserver` to capture execution events for telemetry or auditing.

[//]: # (License and contribution guidelines are kept in their respective files.)
