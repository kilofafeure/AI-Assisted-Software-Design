using ApplicationLayer;
using Data;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using ProcessQueue;
using ProcessQueue.Strategies;

var services = new ServiceCollection();

services.AddSingleton<ItemMemoryRepository>();
services.AddSingleton<IGet>(provider => provider.GetRequiredService<ItemMemoryRepository>());
services.AddSingleton<ISave>(provider => provider.GetRequiredService<ItemMemoryRepository>());
services.AddSingleton<ITaskStrategy, HttpTaskStrategy>();
services.AddSingleton<ITaskStrategy, FileTaskStrategy>();
services.AddSingleton<IQueue, TaskProcessQueue>();
services.AddTransient<GetUseCase>();
services.AddTransient<SaveUseCase>();

await using var serviceProvider = services.BuildServiceProvider();
var saveUseCase = serviceProvider.GetRequiredService<SaveUseCase>();
var getUseCase = serviceProvider.GetRequiredService<GetUseCase>();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Crear tarea");
    Console.WriteLine("2. Mostrar tareas");
    Console.WriteLine("3. Salir");
    Console.Write("Selecciona una opción: ");

    var option = Console.ReadLine()?.Trim();

    switch (option)
    {
        case "1":
            await CreateTaskAsync();
            break;
        case "2":
            await ShowTasksAsync();
            break;
        case "3":
        case null:
            return;
        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
}

async Task CreateTaskAsync()
{
    Console.WriteLine("Tipo de tarea:");
    Console.WriteLine("1. HTTP");
    Console.WriteLine("2. Archivo");
    Console.Write("Selecciona un tipo: ");

    TaskType? taskType = Console.ReadLine()?.Trim() switch
    {
        "1" => TaskType.HTTP,
        "2" => TaskType.File,
        _ => null
    };

    if (taskType is null)
    {
        Console.WriteLine("Tipo de tarea no válido.");
        return;
    }

    Console.Write("Nombre: ");
    var name = Console.ReadLine();
    if (name is null)
    {
        return;
    }

    Console.Write("Data: ");
    var data = Console.ReadLine();
    if (data is null)
    {
        return;
    }

    TaskItem taskItem;
    try
    {
        taskItem = new TaskItem
        {
            Id = Guid.NewGuid(),
            Name = name,
            Status = false,
            Data = data,
            Type = taskType.Value
        };
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine($"No se pudo crear la tarea: {exception.Message}");
        return;
    }

    var enqueued = await saveUseCase.ExecuteAsync(taskItem);
    Console.WriteLine(enqueued
        ? "Tarea guardada y encolada para procesarse."
        : "Tarea guardada, pero no se pudo encolar.");
}

async Task ShowTasksAsync()
{
    var taskItems = await getUseCase.ExecuteAsync();
    if (taskItems.Count == 0)
    {
        Console.WriteLine("No hay tareas guardadas.");
        return;
    }

    foreach (var taskItem in taskItems)
    {
        Console.WriteLine($"Nombre: {taskItem.Name} | Data: {taskItem.Data} | Status: {taskItem.Status}");
    }
}
