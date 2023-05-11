using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Interfaces;

public interface IBoard
{
    Tile GetTile(BoardCoordinate coordinate);
    Tile? GetAdjacentTile(BoardCoordinate fromCoordinate, Direction direction);
}