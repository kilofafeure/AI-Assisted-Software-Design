using Domain;

namespace ApplicationLayer;

public class SaveUseCase(ISave save, IQueue queue)
{
    private readonly ISave _save = save ?? throw new ArgumentNullException(nameof(save));
    private readonly IQueue _queue = queue ?? throw new ArgumentNullException(nameof(queue));

    public async Task ExecuteAsync(TaskItem taskItem)
    {
        await _save.Save(taskItem);
        await _queue.EnqueueAsync(taskItem);
    }
}
