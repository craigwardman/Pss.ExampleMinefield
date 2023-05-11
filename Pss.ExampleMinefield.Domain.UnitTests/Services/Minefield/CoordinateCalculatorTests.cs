using FluentAssertions;
using FluentAssertions.Execution;
using Pss.ExampleMinefield.Domain.Model.Minefield;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.UnitTests.Services.Minefield;

[TestFixture]
public class CoordinateCalculatorTests
{
    [TestCase(Direction.Up)]
    [TestCase(Direction.Left)]
    public void GetAdjacentCoordinate_WhenAlreadyAtMin_UpAndLeftReturnsNull(Direction direction)
    {
        var coordinate = new BoardCoordinate(BoardCoordinate.Min, BoardCoordinate.Min);
        var result = new CoordinateCalculator().GetAdjacentCoordinate(coordinate, direction);

        result.Should().BeNull();
    }

    [TestCase(Direction.Down)]
    [TestCase(Direction.Right)]
    public void GetAdjacentCoordinate_WhenAlreadyAtMax_DownAndRightReturnsNull(Direction direction)
    {
        var coordinate = new BoardCoordinate(BoardCoordinate.Max, BoardCoordinate.Max);
        var result = new CoordinateCalculator().GetAdjacentCoordinate(coordinate, direction);

        result.Should().BeNull();
    }

    [TestCase(Direction.Up, 0, -1)]
    [TestCase(Direction.Left, -1, 0)]
    [TestCase(Direction.Down, 0, 1)]
    [TestCase(Direction.Right, 1, 0)]
    public void GetAdjacentCoordinate_WhenPossible_ReturnsExpected(Direction direction, int offsetX, int offsetY)
    {
        var coordinate = new BoardCoordinate(1, 1);
        var result = new CoordinateCalculator().GetAdjacentCoordinate(coordinate, direction);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result!.X.Should().Be(coordinate.X + offsetX);
            result!.Y.Should().Be(coordinate.Y + offsetY);
        }
    }

    [Test]
    public void GetAdjacentCoordinate_DirectionOutOfRange_Throws()
    {
        const Direction invalidDirection = (Direction)99;
        var act = () => new CoordinateCalculator().GetAdjacentCoordinate(new BoardCoordinate(0, 0), invalidDirection);

        act.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("direction");
    }
}