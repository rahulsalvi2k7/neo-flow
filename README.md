# neo-flow

> Extensible, performant, and lightweight workflow engine for .NET 8.0

Neo-flow is a modern workflow orchestration library that enables you to build complex, composable business processes with a clean, fluent API. It supports sequential execution, conditional branching, parallel processing, and flexible composition patterns.

## Features

- **Lightweight & Fast**: Minimal overhead with high-performance async/await support
- **Composable**: Build complex workflows by combining simple, reusable steps
- **Type-Safe**: Full C# type checking with nullable reference types enabled
- **Async-First**: Built on modern async patterns with cancellation support
- **Extensible**: Implement custom steps, conditions, and observers for your domain
- **Observability**: Built-in step lifecycle observers for monitoring and auditing

## Getting Started

### Installation

Add the neo-flow NuGet package to your project:

```bash
dotnet add package neo.flow.core
```

### Quick Example

```csharp
using neo.flow.core;
using neo.flow.core.Builder;
using neo.flow.core.Engine;
using neo.flow.core.Interfaces;

// Define your custom steps
public class LogStep : IBusinessStep
{
    public string Name => "Log";
    
    public async Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
    {
        Console.WriteLine("Processing step executed");
        await Task.CompletedTask;
    }
}

// Build a workflow
var workflow = new WorkflowBuilder("MyWorkflow")
    .Step(new LogStep())
    .Build();

// Execute it
var context = new ExecutionContext();
var engine = new WorkflowEngine(stepObserver: null);

await engine.RunAsync(workflow, context);
```

## Core Concepts

### Business Steps

A `IBusinessStep` is the fundamental unit of work in neo-flow. Implement this interface to create custom steps:

```csharp
public interface IBusinessStep
{
    string Name { get; }
    Task ExecuteAsync(IExecutionContext context, CancellationToken ct);
}
```

### Step Composition

Neo-flow provides built-in composite steps for common patterns:

- **Sequential Steps**: Execute steps one after another using `WorkflowBuilder`
- **Parallel Steps**: Execute multiple steps concurrently with `ParallelStep`
- **Conditional Steps**: Branch execution based on conditions with `ConditionalStep`
- **Conditional Parallel Steps**: Execute multiple branches conditionally with `ConditionalParallelStep`
- **Switch Steps**: Route to one of many steps based on condition matching with `SwitchStep`

### Conditions

Implement `ICondition` to define custom decision logic:

```csharp
public interface ICondition
{
    bool Evaluate(IExecutionContext context);
}
```

### Execution Context

The `IExecutionContext` carries state through workflow execution. It allows steps to share data and coordinate their behavior.

### Step Observers

Monitor workflow execution with `IStepObserver`:

```csharp
public interface IStepObserver
{
    Task OnStepStarted(string stepName);
    Task OnStepCompleted(string stepName);
    Task OnStepFailed(string stepName);
}
```

## Building Workflows

### Sequential Workflow

Chain steps together in order:

```csharp
var workflow = new WorkflowBuilder("SequentialWorkflow")
    .Step(step1)
    .Step(step2)
    .Step(step3)
    .Build();
```

### Parallel Workflow

Execute multiple steps concurrently (limited to 5 concurrent tasks):

```csharp
var parallelWorkflow = new ParallelStep("ProcessInParallel", 
    step1, step2, step3);
```

### Conditional Workflow

Execute different steps based on conditions:

```csharp
var conditionalWorkflow = new ConditionalStep(
    condition: myCondition,
    thenStep: positiveStep,
    elseStep: negativeStep);
```

### Switch Workflow

Route to matching cases with an optional default:

```csharp
var switchWorkflow = new SwitchStep("Router",
    cases: new List<(ICondition, IBusinessStep)>
    {
        (condition1, step1),
        (condition2, step2),
        (condition3, step3)
    },
    defaultStep: fallbackStep);
```

### Conditional Parallel Workflow

Execute multiple conditional branches in parallel:

```csharp
var branches = new List<(ICondition, IBusinessStep)>
{
    (condition1, branch1),
    (condition2, branch2),
    (condition3, branch3)
};

var parallelConditional = new ConditionalParallelStep("ProcessBranches", branches);
```

## Execution Model

The `WorkflowEngine` orchestrates workflow execution:

```csharp
var engine = new WorkflowEngine(stepObserver: myObserver);

try
{
    var context = new ExecutionContext();
    var cancellationToken = CancellationToken.None;
    
    await engine.RunAsync(workflow, context, cancellationToken);
}
catch (Exception ex)
{
    // Handle workflow errors
}
```

## Architecture

Neo-flow follows clean architecture principles:

- **Interfaces** (`IBusinessStep`, `ICondition`, `IExecutionContext`): Define the contract for extensibility
- **Steps** (`ConditionalStep`, `ParallelStep`, etc.): Implement composition patterns
- **Engine** (`WorkflowEngine`): Orchestrates execution and notifies observers
- **Builder** (`WorkflowBuilder`, etc.): Provides fluent API for workflow construction
- **Models** (`ExecutionContext`, `AuditEntry`): Support workflow state and auditing

## Testing

Neo-flow includes comprehensive unit tests using NUnit and Moq:

```bash
cd tests/neo.flow.core.tests
dotnet test
```

The test suite covers all step types, edge cases, and error conditions.

## Requirements

- .NET 8.0 or later
- C# 11 or later

## Project Structure

```
neo-flow/
├── src/neo-flow/neo.flow.core/
│   ├── Interfaces/           # Core abstractions
│   ├── Steps/                # Composite step implementations
│   ├── Engine/               # Workflow orchestration
│   ├── Builder/              # Fluent API builders
│   ├── Models/               # Data models
│   └── Base/                 # Base classes
├── tests/neo.flow.core.tests/
│   └── Steps/                # Comprehensive unit tests
└── README.md
```

## Performance Considerations

- **Lightweight**: Minimal allocation and GC pressure
- **Parallel Execution**: Uses `Parallel.ForEachAsync` for efficient concurrent processing
- **Async All The Way**: Full async/await support prevents blocking
- **Cancellation Support**: Respects CancellationToken for graceful shutdown

## License

This project is licensed under the GPL-3.0 License. See the [LICENSE](LICENSE) file for details.
