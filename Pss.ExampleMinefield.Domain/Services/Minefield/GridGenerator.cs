using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Services.Minefield;

internal class GridGenerator : IGridGenerator
{
    private readonly IMineGenerator _mineGenerator;

    public GridGenerator(IMineGenerator mineGenerator)
    {
        _mineGenerator = mineGenerator ?? throw new ArgumentNullException(nameof(mineGenerator));
    }

    public Tile[,] GenerateGridTiles()
    {
        var grid = new Tile[BoardCoordinate.Max, BoardCoordinate.Max];
        for (var x = 0; x < BoardCoordinate.Max; x++)
        {
            for (var y = 0; y < BoardCoordinate.Max; y++)
            {
                grid[x, y] = new Tile(new BoardCoordinate(x, y), _mineGenerator);
            }
        }

        return grid;
    }
}