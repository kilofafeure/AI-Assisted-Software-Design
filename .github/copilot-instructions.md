# Copilot instructions

## Architecture

This repository contains two separate .NET examples:

- `Interfaces/FileGenerator` demonstrates interface-based number generation and saving in one console project. `Program.cs` is its composition root, and `NumberUseCase` passes generated numbers to an `ISave` implementation. The active generator produces the first 10 prime numbers; `JSONSave` writes `numbers.json` to the process working directory. Its `ISave` is separate from the Tasks project's `ISave`.
- `Responsibilities/Tasks.slnx` is a hexagonal-style Tasks example. `Domain` owns `TaskItem` and `TaskType`; `ApplicationLayer` owns the `IGet`, `ISave`, and `IQueue` ports and the `GetUseCase`/`SaveUseCase` orchestration. `Data` implements `IGet` and `ISave`; `ProcessQueue` implements `IQueue`; `ConsoleApp` composes these projects and provides the menu.

In the Tasks flow, `SaveUseCase` awaits persistence before enqueueing the same item. `GetUseCase` delegates to `IGet`. Keep dependencies directed toward the domain and application contracts: adapters implement application ports, while `ConsoleApp` wires the adapters and use cases together.

For `FileGenerator`-specific work, also follow `Interfaces/.github/copilot-instructions.md`.

## Build and run

The projects target .NET 10. Run commands from the repository root:

```powershell
dotnet build "Responsibilities\Tasks.slnx"
dotnet run --project "Responsibilities\ConsoleApp\ConsoleApp.csproj"
dotnet build "Interfaces\FileGenerator\FileGenerator.csproj"
dotnet run --project "Interfaces\FileGenerator\FileGenerator.csproj"
```

## Repository-specific conventions

- Project files enable nullable reference types and implicit usings and target `net10.0`.
- `TaskItem` enforces its invariants in property setters: `Id` cannot be empty, `Name` cannot be blank and is limited to 50 characters, `Data` cannot be blank, and `Type` must be a defined `TaskType`. `TaskType` currently contains only `HTTP`; preserve these domain rules when changing the entity.
- Application ports use `Task`-based signatures. Use cases expose `ExecuteAsync`; `SaveUseCase` preserves the save-then-enqueue order.
- `ConsoleApp/Program.cs` is the Tasks composition root. `ItemMemoryRepository` is registered once as a singleton and exposed as both `IGet` and `ISave`, so both ports share the same in-memory list. Keep that shared instance when changing registrations.
- The in-memory repository protects its list with a lock and returns a list copy from `Get`; `ConsoleQueue` writes only `TaskItem.Data` to the console.
- Existing source folders use lowercase or mixed-case names such as `interface`, `useCases`, `interfaces`, and `implementations`. Preserve the casing and project-local structure when adding related files.
