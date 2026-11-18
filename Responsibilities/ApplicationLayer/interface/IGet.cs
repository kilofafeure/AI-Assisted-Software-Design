using Domain;

namespace ApplicationLayer;

public interface IGet
{
    Task<List<TaskItem>> Get();
}
