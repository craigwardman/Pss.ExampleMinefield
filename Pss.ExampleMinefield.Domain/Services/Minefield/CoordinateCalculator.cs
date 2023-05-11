using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Services.Minefield;

internal class CoordinateCalculator : ICoordinateCalculator
{
    public BoardCoordinate? GetAdjacentCoordinate(BoardCoordinate startingPoint, Direction direction)
    {
        return direction switch
        {
            Direction.Up => startingPoint.Y - 1 >= BoardCoordinate.Min ? new BoardCoordinate(startingPoint.X, startingPoint.Y - 1) : null,
            Direction.Down => startingPoint.Y + 1 <= BoardCoordinate.Max ? new BoardCoordinate(startingPoint.X, startingPoint.Y + 1) : null,
            Direction.Left => startingPoint.X - 1 >= BoardCoordinate.Min ? new BoardCoordinate(startingPoint.X - 1, startingPoint.Y) : null,
            Direction.Right => startingPoint.X + 1 <= BoardCoordinate.Max ? new BoardCoordinate(startingPoint.X + 1, startingPoint.Y) : null,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}