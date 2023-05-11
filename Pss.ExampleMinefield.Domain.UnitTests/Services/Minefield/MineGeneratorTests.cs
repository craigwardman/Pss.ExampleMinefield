using FluentAssertions;
using Pss.ExampleMinefield.Domain.Model.Minefield;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.UnitTests.Services.Minefield;

[TestFixture]
public class MineGeneratorTests
{
    [TestCase(BoardCoordinate.Min, BoardCoordinate.Min)]
    [TestCase(BoardCoordinate.Max, BoardCoordinate.Max)]
    [Repeat(100)]
    public void ShouldLayMine_WhenCoordinateIsStartOrEndPoint_NeverLaysAMine(int x, int y)
    {
        var coordinate = new BoardCoordinate(x, y);
        var result = new MineGenerator().ShouldLayMine(coordinate);

        result.Should().BeFalse();
    }
    
    [Test]
    public void ShouldLayMine_WhenCoordinateIsNotStartOrEndPoint_LaysAMineAround25pctOfTheTime()
    {
        var coordinate = new BoardCoordinate(1, 1);

        var results = new List<bool>();
        for (var i = 0; i < 1000; i++)
        {
            results.Add(new MineGenerator().ShouldLayMine(coordinate));
    
        }

        (results.Count(r => r) / 1000D * 100).Should().BeInRange(20, 30);
    }
}