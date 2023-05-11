using FluentAssertions;
using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.Domain.UnitTests.Model.Minefield;

[TestFixture]
public class BoardCoordinateTests
{
    [TestCase(-1)]
    [TestCase(8)]
    public void Ctor_XOutOfRange_Throws(int x)
    {
        var act = () => new BoardCoordinate(x, 0);

        act.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("X");
    }
    
    [TestCase(-1)]
    [TestCase(8)]
    public void Ctor_YOutOfRange_Throws(int y)
    {
        var act = () => new BoardCoordinate(0, y);

        act.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("Y");
    }

    [TestCase(0, "A")]
    [TestCase(1, "B")]
    [TestCase(2, "C")]
    [TestCase(3, "D")]
    [TestCase(4, "E")]
    [TestCase(5, "F")]
    [TestCase(6, "G")]
    [TestCase(7, "H")]
    public void ToString_WhenCalled_ReturnsExpected(int coord, string alpha)
    {
        var sut = new BoardCoordinate(coord, coord);

        sut.ToString().Should().Be($"{alpha}{coord + 1}");
    }
}