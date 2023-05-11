using Microsoft.Extensions.DependencyInjection;
using Pss.ExampleMinefield.Domain.Interfaces;
using Pss.ExampleMinefield.Domain.Model.Game;
using Pss.ExampleMinefield.Domain.Model.Minefield;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IGridGenerator, GridGenerator>();
        services.AddTransient<ICoordinateCalculator, CoordinateCalculator>();
        services.AddTransient<IBoard, Board>();
        services.AddTransient<IMineGenerator, MineGenerator>();

        services.AddTransient<IGame, Game>();
    }
}