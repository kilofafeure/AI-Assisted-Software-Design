using FileGenerator.Interfaces;

namespace FileGenerator.UseCases;

public class NumberUseCase
{
    private readonly IGenerator _generator;
    private readonly ISave _save;

    public NumberUseCase(IGenerator generator, ISave save)
    {
        _generator = generator;
        _save = save;
    }

    public void Execute()
    {
        var numbers = _generator.Generator();
        _save.Save(numbers);
    }
}
