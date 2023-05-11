using Pss.ExampleMinefield.Domain.Interfaces;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.Model.Minefield;

public class Board : IBoard
{
    private readonly ICoordinateCalculator _coordinateCalculator;
    private readonly Tile[,] _grid;

    public Board(IGridGenerator gridGenerator, ICoordinateCalculator coordinateCalculator)
    {
        if (gridGenerator == null) throw new ArgumentNullException(nameof(gridGenerator));
        _coordinateCalculator = coordinateCalculator ?? throw new ArgumentNullException(nameof(coordinateCalculator));
        _grid = gridGenerator.GenerateGridTiles();
    }

    public Tile GetTile(BoardCoordinate coordinate)
    {
        return _grid[coordinate.X, coordinate.Y];
    }

    public Tile? GetAdjacentTile(BoardCoordinate fromCoordinate, Direction direction)
    {
        var adjacentCoordinate = _coordinateCalculator.GetAdjacentCoordinate(fromCoordinate, direction);
        return adjacentCoordinate != null ? _grid[adjacentCoordinate.X, adjacentCoordinate.Y] : null;
    }
}