using Pss.ExampleMinefield.Domain.Interfaces;
using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Model.Game;

public class Game : IGame
{
    private readonly IBoard _board;
    private readonly Player _player;

    public Game(IBoard board)
    {
        _board = board ?? throw new ArgumentNullException(nameof(board));
        _player = new Player(board.GetTile(new BoardCoordinate(0, 0)));
    }

    public void Move(Direction direction)
    {
        var nextTile = _board.GetAdjacentTile(_player.CurrentTile.Coordinate, direction);
        if (nextTile != null)
        {
            _player.MoveToTile(nextTile);
        }
    }

    public bool IsGameOver() => !_player.IsAlive() || _player.HasWon();

    public string StateOfPlay()
    {
        return _player.ToString();
    }
}