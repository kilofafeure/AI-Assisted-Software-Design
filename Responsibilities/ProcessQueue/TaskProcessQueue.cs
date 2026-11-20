using System.Threading.Channels;
using ApplicationLayer;
using Domain;

namespace ProcessQueue;

public sealed class TaskProcessQueue : IQueue, IAsyncDisposable
{
    private readonly Channel<(TaskItem TaskItem, ITaskStrategy Strategy)> _channel =
        Channel.CreateBounded<(TaskItem TaskItem, ITaskStrategy Strategy)>(new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        });

    private readonly ISave _save;
    private readonly IReadOnlyDictionary<TaskType, ITaskStrategy> _strategies;
    private readonly Task _consumerTask;

    public TaskProcessQueue(ISave save, IEnumerable<ITaskStrategy> strategies)
    {
        _save = save ?? throw new ArgumentNullException(nameof(save));
        ArgumentNullException.ThrowIfNull(strategies);
        _strategies = strategies.ToDictionary(strategy => strategy.Type);
        _consumerTask = ProcessQueueAsync();
    }

    public async Task<bool> EnqueueAsync(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        if (!_strategies.TryGetValue(taskItem.Type, out var strategy))
        {
            throw new NotSupportedException($"Task type '{taskItem.Type}' is not supported.");
        }

        try
        {
            await _channel.Writer.WriteAsync((taskItem, strategy));
            return true;
        }
        catch (ChannelClosedException)
        {
            return false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        _channel.Writer.TryComplete();
        await _consumerTask;
    }

    private async Task ProcessQueueAsync()
    {
        await foreach (var queuedTask in _channel.Reader.ReadAllAsync())
        {
            queuedTask.TaskItem.Status = await queuedTask.Strategy.ExecuteAsync(queuedTask.TaskItem);
            await _save.Save(queuedTask.TaskItem);
        }
    }
}
