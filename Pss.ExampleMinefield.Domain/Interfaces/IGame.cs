using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.Interfaces;

public interface IGame
{
    void Move(Direction direction);
    bool IsGameOver();
    string StateOfPlay();
}