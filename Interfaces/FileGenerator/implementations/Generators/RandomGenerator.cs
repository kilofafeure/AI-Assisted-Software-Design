using FileGenerator.Interfaces;

namespace FileGenerator.Implementations.Generators;

public class RandomGenerator(int count = 10) : IGenerator
{
    public List<int> Generator()
    {
        var numbers = new List<int>(count);

        for (var i = 0; i < count; i++)
        {
            numbers.Add((int)Random.Shared.NextInt64(int.MinValue, (long)int.MaxValue + 1));
        }

        return numbers;
    }
}
