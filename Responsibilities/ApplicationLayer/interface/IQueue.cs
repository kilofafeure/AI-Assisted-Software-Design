using Domain;

namespace ApplicationLayer;

public interface IQueue
{
    Task EnqueueAsync(TaskItem taskItem);
}
