using Domain;

namespace ApplicationLayer;

public interface ITaskStrategy
{
    TaskType Type { get; }

    Task<bool> ExecuteAsync(TaskItem taskItem);
}
