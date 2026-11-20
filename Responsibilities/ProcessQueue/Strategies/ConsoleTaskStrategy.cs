using ApplicationLayer;
using Domain;

namespace ProcessQueue.Strategies;

public sealed class ConsoleTaskStrategy : ITaskStrategy
{
    public TaskType Type => TaskType.CONSOLE;

    public Task<bool> ExecuteAsync(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        Console.WriteLine(taskItem.Name);
        Console.WriteLine(taskItem.Data);
        return Task.FromResult(true);
    }
}
