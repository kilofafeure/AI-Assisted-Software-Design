using FileGenerator.Implementations.Generators;
using FileGenerator.Implementations.Savers;
using FileGenerator.Interfaces;
using FileGenerator.UseCases;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddTransient<IGenerator, PrimeNumberGenerator>();
services.AddTransient<ISave>(_ => new JSONSave("numbers.json"));
// Previous implementations:
// services.AddTransient<IGenerator, RandomGenerator>();
// services.AddTransient<ISave, PipeSave>();
services.AddTransient<NumberUseCase>();

using var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetRequiredService<NumberUseCase>().Execute();
