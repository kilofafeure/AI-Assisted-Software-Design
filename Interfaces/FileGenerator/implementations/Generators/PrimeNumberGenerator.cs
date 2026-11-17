using FileGenerator.Interfaces;

namespace FileGenerator.Implementations.Generators;

public class PrimeNumberGenerator(int count = 10) : IGenerator
{
    public List<int> Generator()
    {
        var numbers = new List<int>(count);
        long candidate = 2;

        while (numbers.Count < count)
        {
            if (candidate > int.MaxValue)
            {
                throw new InvalidOperationException("The requested count exceeds the number of primes representable as Int32.");
            }

            var number = (int)candidate;
            if (IsPrime(number))
            {
                numbers.Add(number);
            }

            candidate = candidate == 2 ? 3 : candidate + 2;
        }

        return numbers;
    }

    private static bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }

        if (number % 2 == 0)
        {
            return number == 2;
        }

        for (var divisor = 3; divisor <= number / divisor; divisor += 2)
        {
            if (number % divisor == 0)
            {
                return false;
            }
        }

        return true;
    }
}
