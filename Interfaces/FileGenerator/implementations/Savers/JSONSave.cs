using System.Text.Json;
using FileGenerator.Interfaces;

namespace FileGenerator.Implementations.Savers;

public class JSONSave : ISave
{
    private readonly string _filePath;

    public JSONSave(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
    }

    public void Save(List<int> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(numbers));
    }
}
