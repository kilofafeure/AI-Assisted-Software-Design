# Copilot instructions for this repository

## Project overview

This repository is a very small .NET solution focused on a single console app: `Interfaces/FileGenerator`.

- Entry point: `Interfaces/FileGenerator/Program.cs`
- Project file: `Interfaces/FileGenerator/FileGenerator.csproj`
- Target framework: `net10.0`
- Build output: executable console app with nullable reference types enabled and implicit usings enabled

The current codebase is intentionally minimal and does not yet contain a solution file, separate libraries, or automated tests. If you add functionality, keep the project structure simple and stay within this executable project unless the work clearly justifies a new project.

## Build, test, and validation commands

Use the .NET CLI directly from the repository root:

- Build the project:
  - `dotnet build "Interfaces/FileGenerator/FileGenerator.csproj"`
- Run the executable:
  - `dotnet run --project "Interfaces/FileGenerator/FileGenerator.csproj"`
- If a test project is added later, run a single test with a filter:
  - `dotnet test --filter "Name~<TestName>"`
  - Example: `dotnet test --filter "Name~ShouldGenerateFile"`

There is no dedicated lint configuration or custom build script in this repository today. The default verification step for this project is `dotnet build`.

## High-level architecture

The repository is organized around a single executable that generates interface-related files:

- `Program.cs` is the program entry point and currently contains the app startup behavior.
- `FileGenerator.csproj` defines the runtime and compiler settings for the console app.
- `interfaces` contains the `IGenerator` and `ISave` contracts.
- `implementations` contains concrete generators and savers.
- `useCases` coordinates the workflow by passing generated numbers to an `ISave`.
- Keep this flow within the project unless a clear multi-project split is required.

This means Copilot sessions should assume a lightweight, single-project architecture rather than a layered application with web/API/service boundaries.

## Key conventions

- Keep the project as a .NET console app unless there is a strong reason to add another project type.
- Prefer adding support code inside `Interfaces/FileGenerator` rather than creating unrelated folders or projects.
- Always create interface source files in `Interfaces/FileGenerator/interfaces` (the `interfaces` folder inside the project).
- Always create concrete implementation source files under `Interfaces/FileGenerator/implementations`; organize them into subfolders there when appropriate.
- Create workflow orchestration classes in `Interfaces/FileGenerator/useCases`.
- The project is configured with `Nullable` enabled and `ImplicitUsings` enabled; maintain null-safety and modern C# conventions when editing files.
- Target framework is `net10.0`; use SDK-compatible APIs and avoid older .NET patterns unless required by the task.
- If the generator grows in complexity, keep the entry point thin and move logic into helper classes or separate files inside the same project rather than spreading logic across multiple top-level projects.
- Do not introduce build/test tooling or repository conventions that are not already present unless the task explicitly requires them.

## Working expectations

When making changes here, prefer surgical edits that fit the existing minimal structure. The repo does not currently include extensive docs or automation, so the most reliable validation path is direct `dotnet build` and targeted `dotnet run` checks for feature-specific behavior.
