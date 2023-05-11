using System.Diagnostics.CodeAnalysis;
using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Model.Game;

public class Player
{
    public const int MaxLives = 3;

    public Player(Tile startingTile)
    {
        MoveToTile(startingTile);
    }

    public int Score { get; private set; }

    public int Lives { get; private set; } = MaxLives;

    public Tile CurrentTile { get; private set; }

    [MemberNotNull(nameof(CurrentTile))]
    public void MoveToTile(Tile tile)
    {
        CurrentTile = tile;
        Score++;
        if (CurrentTile.HasMine) Lives--;
    }

    public bool IsAlive()
    {
        return Lives > 0;
    }

    public bool HasWon()
    {
        return CurrentTile.Coordinate is { X: BoardCoordinate.Max, Y: BoardCoordinate.Max };
    }

    public override string ToString()
    {
        if (!IsAlive()) return $"Died with a score of {Score}";

        if (HasWon()) return $"Won with a score of {Score}";

        return $"Current Position: {CurrentTile.Coordinate}, Score: {Score}, Remaining Lives: {Lives}";
    }
}