using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Services.Minefield;

public interface ICoordinateCalculator
{
    BoardCoordinate? GetAdjacentCoordinate(BoardCoordinate startingPoint, Direction direction);
}