using ApplicationLayer;
using Domain;

namespace Data;

public class ItemMemoryRepository : ISave, IGet
{
    private readonly List<TaskItem> _items = [];
    private readonly object _syncRoot = new();

    public async Task Save(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        await Task.CompletedTask;

        lock (_syncRoot)
        {
            _items.Add(taskItem);
        }
    }

    public async Task<List<TaskItem>> Get()
    {
        await Task.CompletedTask;

        lock (_syncRoot)
        {
            return [.. _items];
        }
    }
}
