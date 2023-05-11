using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Services.Minefield;

public interface IGridGenerator
{
    Tile[,] GenerateGridTiles();
}