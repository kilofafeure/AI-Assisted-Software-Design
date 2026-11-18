using Domain;

namespace ApplicationLayer;

public interface IQueue
{
    /// <summary>
    /// Enqueues the item. The result indicates acceptance, not processing success.
    /// </summary>
    Task<bool> EnqueueAsync(TaskItem taskItem);
}
