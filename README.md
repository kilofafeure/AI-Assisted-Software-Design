# AI-Assisted Software Design

Este repositorio contiene ejemplos didácticos de diseño de software en .NET, organizados mediante interfaces, casos de uso e inyección de dependencias:

- `Interfaces/FileGenerator`: una aplicación de consola que genera números y los guarda.
- `Responsibilities/Tasks.slnx`: una solución multiproyecto que implementa una aplicación de tareas con arquitectura hexagonal.

Los proyectos utilizan .NET 10.

## Proyecto FileGenerator

La aplicación muestra cómo separar los contratos de sus implementaciones y conectarlos mediante inyección de dependencias:

- `IGenerator` define la generación de una lista de números. La implementación activa, `PrimeNumberGenerator`, produce los primeros 10 números primos por defecto.
- `ISave` define cómo guardar esa lista. La implementación activa, `JSONSave`, la serializa como un arreglo JSON.
- `NumberUseCase` coordina el flujo: solicita los números al generador y entrega el resultado al componente de guardado.
- `Program.cs` registra `PrimeNumberGenerator`, `JSONSave` y `NumberUseCase` en el contenedor de inyección de dependencias y ejecuta el caso de uso.

También hay implementaciones alternativas (`RandomGenerator` y `PipeSave`); actualmente no están activadas en el registro de `Program.cs`.

El guardado actual escribe `numbers.json` en el directorio de trabajo desde el que se ejecuta la aplicación. Con la configuración predeterminada, el archivo contiene los primeros 10 números primos, por ejemplo:

```json
[2,3,5,7,11,13,17,19,23,29]
```

## Solución Tasks (`Responsibilities`)

La solución `Responsibilities/Tasks.slnx` separa el dominio, los casos de uso y sus adaptadores:

| Proyecto | Responsabilidad |
| --- | --- |
| `Domain` | Define `TaskItem` con `Guid Id`, `string Name`, `bool Status`, `string Data` y `TaskType Type`. Los setters validan que `Id` no sea `Guid.Empty`, que `Name` y `Data` no estén vacíos, que `Name` no supere los 50 caracteres y que `Type` sea un valor definido. Actualmente `TaskType` solo contiene `HTTP`. |
| `ApplicationLayer` | Define `ISave.Save(TaskItem)`, `IGet.Get()` e `IQueue.EnqueueAsync(TaskItem)`, todos con contratos basados en `Task`, además de los casos de uso `SaveUseCase` y `GetUseCase`. |
| `Data` | Implementa `ISave` e `IGet` en `ItemMemoryRepository`, que guarda tareas en una lista en memoria y devuelve una copia de la lista al consultar. |
| `ProcessQueue` | Implementa `IQueue` en `ConsoleQueue`, que escribe en consola el valor de `TaskItem.Data`. |
| `ConsoleApp` | Es el punto de composición: registra las implementaciones y los casos de uso mediante inyección de dependencias, y proporciona el menú interactivo. |

`SaveUseCase` guarda primero la tarea y después la envía a la cola; `GetUseCase` delega la consulta a `IGet`. En `ConsoleApp`, `ItemMemoryRepository` se registra como una única instancia compartida por `IGet` e `ISave`, para que las tareas guardadas puedan consultarse durante la ejecución.

El menú de `ConsoleApp` ofrece estas opciones:

1. Crear una tarea solicitando nombre, datos y estado (`true` o `false`). La aplicación genera el `Guid`, asigna `TaskType.HTTP` y ejecuta `SaveUseCase`.
2. Mostrar las tareas guardadas con su nombre y estado.
3. Salir.

El repositorio es solo en memoria: las tareas dejan de estar disponibles al cerrar la aplicación.

## Prompts

`Interfaces/FileGenerator/prompts/` contiene los prompts de ese ejemplo. `Responsibilities/prompts/` contiene los prompts de la solución Tasks y el diagrama `Arquitectura del proyecto.png`.

## Compilar y ejecutar

Se requiere el SDK de .NET 10. Ejecuta los comandos desde la raíz del repositorio:

```powershell
dotnet build "Responsibilities\Tasks.slnx"
dotnet run --project "Responsibilities\ConsoleApp\ConsoleApp.csproj"

dotnet build "Interfaces\FileGenerator\FileGenerator.csproj"
dotnet run --project "Interfaces\FileGenerator\FileGenerator.csproj"
```

`ConsoleApp` inicia el menú de tareas. `FileGenerator` crea o sobrescribe `numbers.json` en el directorio de trabajo desde el que se ejecuta.

## `.gitignore`

El `.gitignore` de la raíz excluye los artefactos de compilación de .NET (`bin/` y `obj/`), archivos de usuario de Visual Studio y los archivos `numbers.txt` y `numbers.json` generados por `FileGenerator`.
