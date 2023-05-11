using Microsoft.Extensions.DependencyInjection;
using Pss.ExampleMinefield.Domain;

namespace Pss.ExampleMinefield.App;

public class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();
        services.AddDomainServices();
        services.AddTransient<IGameCli, GameCli>();
        services.AddTransient<IConsoleWrapper, DotnetConsoleWrapper>();
        
        var serviceProvider = services.BuildServiceProvider();
        var gameCli = serviceProvider.GetRequiredService<IGameCli>();

        gameCli.Start();
    }
}