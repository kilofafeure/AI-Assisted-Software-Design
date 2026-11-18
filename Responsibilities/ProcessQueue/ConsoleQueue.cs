using ApplicationLayer;
using Domain;

namespace ProcessQueue;

public class ConsoleQueue : IQueue
{
    public async Task EnqueueAsync(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        await Console.Out.WriteLineAsync(taskItem.Data);
    }
}
