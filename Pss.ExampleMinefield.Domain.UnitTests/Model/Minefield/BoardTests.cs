using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using Pss.ExampleMinefield.Domain.Model.Minefield;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.UnitTests.Model.Minefield;

[TestFixture]
public class BoardTests
{
    [SetUp]
    public void SetUp()
    {
        _gridGeneratorMock.Reset();
        _gridGeneratorMock.Setup(g => g.GenerateGridTiles()).Returns(_stubGrid);

        _coordinateCalculatorMock.Reset();
    }

    private readonly Fixture _fixture = new();
    private readonly Mock<IGridGenerator> _gridGeneratorMock = new();
    private readonly Mock<ICoordinateCalculator> _coordinateCalculatorMock = new();
    private readonly Tile[,] _stubGrid;

    public BoardTests()
    {
        _fixture.Customize(new AutoMoqCustomization());
        _fixture.Customize<BoardCoordinate>(composer => composer.FromFactory(() => new BoardCoordinate(
            _fixture.Create<int>() % 8,
            _fixture.Create<int>() % 8)));
        
        _stubGrid = new[,]
        {
            { _fixture.Create<Tile>(), _fixture.Create<Tile>() }, { _fixture.Create<Tile>(), _fixture.Create<Tile>() }
        };
    }

    [Test]
    public void Ctor_ParamNull_Throws()
    {
        new GuardClauseAssertion(_fixture)
            .Verify(typeof(Board).GetConstructors());
    }

    [Test]
    public void Ctor_WhenCalled_CreatesMaxSizedGrid()
    {
        _ = GetDefaultSut();

        _gridGeneratorMock.Verify(g => g.GenerateGridTiles());
    }

    [Test]
    public void GetTile_WhenCalled_ReturnsTileAtCoordinate()
    {
        var sut = GetDefaultSut();
        var result = sut.GetTile(new BoardCoordinate(1, 1));

        result.Should().Be(_stubGrid[1, 1]);
    }

    [Test]
    public void GetAdjacentTile_WhenAdjacentCoordinateExists_ReturnsTileAtAdjacentCoordinate()
    {
        _coordinateCalculatorMock
            .Setup(c => c.GetAdjacentCoordinate(It.IsAny<BoardCoordinate>(), It.IsAny<Direction>()))
            .Returns(new BoardCoordinate(1, 1));
        var sut = GetDefaultSut();
        
        var result = sut.GetAdjacentTile(_fixture.Create<BoardCoordinate>(), _fixture.Create<Direction>());

        result.Should().Be(_stubGrid[1, 1]);
    }
    
    [Test]
    public void GetAdjacentTile_WhenAdjacentCoordinateDoesNotExist_ReturnsNull()
    {
        _coordinateCalculatorMock
            .Setup(c => c.GetAdjacentCoordinate(It.IsAny<BoardCoordinate>(), It.IsAny<Direction>()))
            .Returns((BoardCoordinate?)null);
        var sut = GetDefaultSut();
        
        var result = sut.GetAdjacentTile(_fixture.Create<BoardCoordinate>(), _fixture.Create<Direction>());

        result.Should().BeNull();
    }

    private Board GetDefaultSut()
    {
        return new Board(_gridGeneratorMock.Object, _coordinateCalculatorMock.Object);
    }
}