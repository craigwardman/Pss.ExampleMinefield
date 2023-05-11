using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using Pss.ExampleMinefield.Domain.Interfaces;
using Pss.ExampleMinefield.Domain.Model.Game;
using Pss.ExampleMinefield.Domain.Model.Minefield;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.UnitTests.Model.Game;

[TestFixture]
public class GameTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IBoard> _boardMock = new();
    private readonly Mock<IMineGenerator> _mineGeneratorMock = new();

    public GameTests()
    {
        _fixture.Customize(new AutoMoqCustomization());
    }

    [SetUp]
    public void SetUp()
    {
        _mineGeneratorMock.Reset();
        
        _boardMock.Reset();
        _boardMock.Setup(b => b.GetTile(new BoardCoordinate(0, 0)))
            .Returns(new Tile(new BoardCoordinate(0, 0), _mineGeneratorMock.Object));
    }

    [Test]
    public void Ctor_NullParam_Throws()
    {
        new GuardClauseAssertion(_fixture)
            .Verify(typeof(Domain.Model.Game.Game).GetConstructors());
    }

    [Test]
    public void StateOfPlay_WhenGameLoads_PlayerIsAtTileA1()
    {
        var sut = GetDefaultSut();

        sut.StateOfPlay().Should().Contain("Current Position: A1");
    }

    [Test]
    public void Move_WhenTileIsNull_DoesNotMove()
    {
        _boardMock.Setup(b => b.GetAdjacentTile(It.IsAny<BoardCoordinate>(), It.IsAny<Direction>()))
            .Returns((Tile?)null);

        var sut = GetDefaultSut();
        sut.Move(_fixture.Create<Direction>());

        sut.StateOfPlay().Should().Contain("Current Position: A1");
    }

    [Test]
    public void Move_WhenTileIsNotNull_StateOfPlayUpdated()
    {
        _boardMock.Setup(b => b.GetAdjacentTile(It.IsAny<BoardCoordinate>(), It.IsAny<Direction>()))
            .Returns(new Tile(new BoardCoordinate(1, 1), _mineGeneratorMock.Object));

        var sut = GetDefaultSut();
        sut.Move(_fixture.Create<Direction>());

        sut.StateOfPlay().Should().Contain("Current Position: B2");
    }

    [Test]
    public void IsGameOver_WhilePlayerIsAliveAndHasNotWon_ReturnsFalse()
    {
        var sut = GetDefaultSut();

        sut.IsGameOver().Should().BeFalse();
    }

    [Test]
    public void IsGameOver_WhilePlayerIsNotAlive_ReturnsTrue()
    {
        _mineGeneratorMock.Setup(m => m.ShouldLayMine(It.IsAny<BoardCoordinate>())).Returns(true);
        _boardMock.Setup(b => b.GetAdjacentTile(It.IsAny<BoardCoordinate>(), It.IsAny<Direction>()))
            .Returns(new Tile(new BoardCoordinate(1, 1), _mineGeneratorMock.Object));

        var sut = GetDefaultSut();
        for (var i = 0; i < Player.MaxLives; i++)
        {
            sut.Move(_fixture.Create<Direction>());
        }

        sut.IsGameOver().Should().BeTrue();
    }

    [Test]
    public void IsGameOver_WhilePlayerHasWon_ReturnsTrue()
    {
        _mineGeneratorMock.Setup(m => m.ShouldLayMine(It.IsAny<BoardCoordinate>())).Returns(false);
        _boardMock.Setup(b => b.GetAdjacentTile(It.IsAny<BoardCoordinate>(), It.IsAny<Direction>()))
            .Returns(new Tile(new BoardCoordinate(7, 7), _mineGeneratorMock.Object));

        var sut = GetDefaultSut();
        sut.Move(_fixture.Create<Direction>());

        sut.IsGameOver().Should().BeTrue();
    }

    private Domain.Model.Game.Game GetDefaultSut() => new(_boardMock.Object);
}