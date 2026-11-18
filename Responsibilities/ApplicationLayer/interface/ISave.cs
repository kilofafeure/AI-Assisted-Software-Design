using Domain;

namespace ApplicationLayer;

public interface ISave
{
    Task Save(TaskItem taskItem);
}
