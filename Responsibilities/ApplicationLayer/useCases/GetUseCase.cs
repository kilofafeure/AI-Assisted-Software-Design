using Domain;

namespace ApplicationLayer;

public class GetUseCase(IGet get)
{
    private readonly IGet _get = get ?? throw new ArgumentNullException(nameof(get));

    public Task<List<TaskItem>> ExecuteAsync()
    {
        return _get.Get();
    }
}
