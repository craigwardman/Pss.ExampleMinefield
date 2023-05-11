using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Pss.ExampleMinefield.Domain.Model.Minefield;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.UnitTests.Services.Minefield;

[TestFixture]
public class GridGeneratorTests
{
    [SetUp]
    public void SetUp()
    {
        _mineGeneratorMock.Reset();
    }

    private readonly Mock<IMineGenerator> _mineGeneratorMock = new();
    private readonly Fixture _fixture = new();

    public GridGeneratorTests()
    {
        _fixture.Customize(new AutoMoqCustomization());
    }

    [Test]
    public void Generate_WhenCalled_ReturnsCorrectSize()
    {
        var sut = GetDefaultSut();

        var grid = sut.GenerateGridTiles();

        grid.Length.Should().Be(BoardCoordinate.Max * BoardCoordinate.Max);
    }

    [Test]
    public void Generate_ForEachTile_AssignsCorrectCoordinate()
    {
        var sut = GetDefaultSut();

        var grid = sut.GenerateGridTiles();

        using (new AssertionScope())
        {
            for (var x = 0; x < BoardCoordinate.Max; x++)
            for (var y = 0; y < BoardCoordinate.Max; y++)
                grid[x, y].Coordinate.Should().BeEquivalentTo(new BoardCoordinate(x, y));
        }
    }


    [TestCase(true)]
    [TestCase(false)]
    public void Generate_ForEachTile_AssignsMineGenerator(bool hasMineValue)
    {
        _mineGeneratorMock.Setup(g => g.ShouldLayMine(It.IsAny<BoardCoordinate>()))
            .Returns(hasMineValue);

        var sut = GetDefaultSut();

        var grid = sut.GenerateGridTiles();

        using (new AssertionScope())
        {
            foreach (var tile in grid) tile.HasMine.Should().Be(hasMineValue);
        }
    }

    private GridGenerator GetDefaultSut()
    {
        return new GridGenerator(_mineGeneratorMock.Object);
    }
}