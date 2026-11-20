using ApplicationLayer;
using Domain;

namespace ProcessQueue.Strategies;

public sealed class FileTaskStrategy : ITaskStrategy
{
    public TaskType Type => TaskType.File;

    public async Task<bool> ExecuteAsync(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        if (Path.GetFileName(taskItem.Name) != taskItem.Name ||
            taskItem.Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            await Console.Error.WriteLineAsync("Name debe ser un nombre de archivo válido.");
            return false;
        }

        var fileName = $"{taskItem.Name}.txt";
        var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

        try
        {
            await File.WriteAllTextAsync(filePath, taskItem.Data);
            return true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            await Console.Error.WriteLineAsync($"No se pudo guardar el archivo '{fileName}': {exception.Message}");
            return false;
        }
    }
}
