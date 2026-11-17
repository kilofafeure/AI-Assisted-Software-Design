using FileGenerator.Interfaces;

namespace FileGenerator.Implementations.Savers;

public class PipeSave : ISave
{
    public void Save(List<int> numbers)
    {
        File.WriteAllText("numbers.txt", string.Join('|', numbers));
    }
}
