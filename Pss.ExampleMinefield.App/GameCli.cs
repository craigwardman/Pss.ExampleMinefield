using Pss.ExampleMinefield.Domain.Interfaces;
using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.App;

public class GameCli : IGameCli
{
    private readonly IConsoleWrapper _consoleWrapper;
    private readonly IGame _game;

    public GameCli(IGame game, IConsoleWrapper consoleWrapper)
    {
        _consoleWrapper = consoleWrapper ?? throw new ArgumentNullException(nameof(consoleWrapper));
        _game = game ?? throw new ArgumentNullException(nameof(game));
    }
    
    public void Start()
    {
        _consoleWrapper.WriteLine(
            "Welcome to the minefield game, use your arrow keys to navigate across the board without hitting a mine. Press Esc to exit.");
        _consoleWrapper.WriteLine(_game.StateOfPlay());

        ConsoleKeyInfo keyInfo;
        while ((keyInfo = _consoleWrapper.ReadKey(true)).Key != ConsoleKey.Escape && !_game.IsGameOver())
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    _game.Move(Direction.Left);
                    break;
                case ConsoleKey.UpArrow:
                    _game.Move(Direction.Up);
                    break;
                case ConsoleKey.RightArrow:
                    _game.Move(Direction.Right);
                    break;
                case ConsoleKey.DownArrow:
                    _game.Move(Direction.Down);
                    break;
            }

            _consoleWrapper.WriteLine(_game.StateOfPlay());
        }
    }
}