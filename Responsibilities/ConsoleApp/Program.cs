using ApplicationLayer;
using Data;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using ProcessQueue;

var services = new ServiceCollection();

services.AddSingleton<ItemMemoryRepository>();
services.AddSingleton<IGet>(provider => provider.GetRequiredService<ItemMemoryRepository>());
services.AddSingleton<ISave>(provider => provider.GetRequiredService<ItemMemoryRepository>());
services.AddSingleton<IQueue, ConsoleQueue>();
services.AddTransient<GetUseCase>();
services.AddTransient<SaveUseCase>();

using var serviceProvider = services.BuildServiceProvider();
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

    Console.Write("Status (true/false): ");
    var statusInput = Console.ReadLine();
    if (!bool.TryParse(statusInput, out var status))
    {
        Console.WriteLine("Status debe ser true o false.");
        return;
    }

    TaskItem taskItem;
    try
    {
        taskItem = new TaskItem
        {
            Id = Guid.NewGuid(),
            Name = name,
            Status = status,
            Data = data,
            Type = TaskType.HTTP
        };
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine($"No se pudo crear la tarea: {exception.Message}");
        return;
    }

    await saveUseCase.ExecuteAsync(taskItem);
    Console.WriteLine("Tarea guardada.");
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
        Console.WriteLine($"Nombre: {taskItem.Name} | Status: {taskItem.Status}");
    }
}
