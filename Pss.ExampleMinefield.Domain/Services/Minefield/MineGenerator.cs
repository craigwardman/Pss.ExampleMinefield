using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Services.Minefield;

public class MineGenerator : IMineGenerator
{
    public bool ShouldLayMine(BoardCoordinate coordinate)
    {
        return coordinate != new BoardCoordinate(0, 0) && coordinate != new BoardCoordinate(7, 7) &&
               new Random().Next(0, 100) < 25;
    }
}