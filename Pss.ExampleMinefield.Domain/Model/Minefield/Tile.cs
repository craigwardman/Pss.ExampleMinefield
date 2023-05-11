using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.Model.Minefield;

public class Tile
{
    public Tile(BoardCoordinate coordinate, IMineGenerator mineGenerator)
    {
        if (mineGenerator == null) throw new ArgumentNullException(nameof(mineGenerator));
        Coordinate = coordinate ?? throw new ArgumentNullException(nameof(coordinate));
        HasMine = mineGenerator.ShouldLayMine(Coordinate);
    }

    public bool HasMine { get; }

    public BoardCoordinate Coordinate { get; }
}