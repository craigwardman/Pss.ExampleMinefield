using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Services.Minefield;

public interface IMineGenerator
{
    bool ShouldLayMine(BoardCoordinate coordinate);
}