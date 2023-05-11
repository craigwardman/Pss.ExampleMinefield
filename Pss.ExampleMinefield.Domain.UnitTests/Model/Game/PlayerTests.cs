using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Pss.ExampleMinefield.Domain.Model.Game;
using Pss.ExampleMinefield.Domain.Model.Minefield;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.UnitTests.Model.Game;

[TestFixture]
public class PlayerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IMineGenerator> _mineGeneratorMock = new();

    public PlayerTests()
    {
        _fixture.Customize(new AutoMoqCustomization());
        _fixture.Customize<BoardCoordinate>(composer => composer.FromFactory(() => new BoardCoordinate(
            _fixture.Create<int>() % 8,
            _fixture.Create<int>() % 8)));
    }

    [SetUp]
    public void SetUp()
    {
        _mineGeneratorMock.Reset();
    }

    [Test]
    public void Ctor_WhenCalled_MovesPlayerToStartingTile()
    {
        var startingTile = _fixture.Create<Tile>();

        var sut = new Player(startingTile);

        sut.CurrentTile.Should().Be(startingTile);
    }

    [Test]
    public void MoveToTile_WhenCalled_MovesPlayerToAnotherTile()
    {
        var startingTile = _fixture.Create<Tile>();
        var nextTile = _fixture.Create<Tile>();

        var sut = new Player(startingTile);
        sut.MoveToTile(nextTile);

        sut.CurrentTile.Should().Be(nextTile);
    }

    [Test]
    public void MoveToTile_WhenTileHasMine_DecreasesLives()
    {
        _mineGeneratorMock.Setup(m => m.ShouldLayMine(It.IsAny<BoardCoordinate>()))
            .Returns(true);

        var sut = new Player(_fixture.Create<Tile>());
        var oldLives = sut.Lives;
        sut.MoveToTile(new Tile(_fixture.Create<BoardCoordinate>(), _mineGeneratorMock.Object));

        sut.Lives.Should().BeLessThan(oldLives);
    }

    [Test]
    public void MoveToTile_WhenTileDoesNotHaveMine_DoesNotDecreaseLives()
    {
        _mineGeneratorMock.Setup(m => m.ShouldLayMine(It.IsAny<BoardCoordinate>()))
            .Returns(false);

        var sut = new Player(_fixture.Create<Tile>());
        var oldLives = sut.Lives;
        sut.MoveToTile(new Tile(_fixture.Create<BoardCoordinate>(), _mineGeneratorMock.Object));

        sut.Lives.Should().Be(oldLives);
    }

    [Test]
    public void MoveToTile_WhenCalled_IncreasesScore()
    {
        var sut = new Player(_fixture.Create<Tile>());
        var oldScore = sut.Score;
        sut.MoveToTile(_fixture.Create<Tile>());

        sut.Score.Should().BeGreaterThan(oldScore);
    }

    [Test]
    public void IsAlive_WhenLivesGt0_ReturnsTrue()
    {
        var sut = new Player(_fixture.Create<Tile>());
        sut.IsAlive().Should().BeTrue();
    }

    [Test]
    public void IsAlive_WhenLivesEq0_ReturnsFalse()
    {
        _mineGeneratorMock.Setup(m => m.ShouldLayMine(It.IsAny<BoardCoordinate>()))
            .Returns(true);

        var sut = new Player(_fixture.Create<Tile>());
        while (sut.Lives > 0)
        {
            sut.MoveToTile(new Tile(_fixture.Create<BoardCoordinate>(), _mineGeneratorMock.Object));
        }

        sut.IsAlive().Should().BeFalse();
    }

    [Test]
    public void HasWon_WhenTileNotMaxExtent_ReturnsFalse()
    {
        var notWinningTile = new Tile(new BoardCoordinate(_fixture.Create<int>() % BoardCoordinate.Max,
            _fixture.Create<int>() % BoardCoordinate.Max), _mineGeneratorMock.Object);
        var sut = new Player(notWinningTile);
        
        sut.HasWon().Should().BeFalse();
    }
    
    [Test]
    public void HasWon_WhenTileIsMaxExtent_ReturnsTrue()
    {
        var winningTile = new Tile(new BoardCoordinate(BoardCoordinate.Max, BoardCoordinate.Max), _mineGeneratorMock.Object);
        var sut = new Player(winningTile);
        
        sut.HasWon().Should().BeTrue();
    }

    [Test]
    public void ToString_WhenGameInPlay_ReturnsExpected()
    {
        var boardCoordinate = new BoardCoordinate(_fixture.Create<int>() % BoardCoordinate.Max,
            _fixture.Create<int>() % BoardCoordinate.Max);
        var notWinningTile = new Tile(boardCoordinate, _mineGeneratorMock.Object);
        var sut = new Player(notWinningTile);

        sut.ToString().Should().Be($"Current Position: {boardCoordinate}, Score: {sut.Score}, Remaining Lives: {sut.Lives}");
    }
    
    [Test]
    public void ToString_WhenPlayerDied_ReturnsExpected()
    {
        _mineGeneratorMock.Setup(m => m.ShouldLayMine(It.IsAny<BoardCoordinate>()))
            .Returns(true);

        var notWinningTile = new Tile(new BoardCoordinate(_fixture.Create<int>() % BoardCoordinate.Max,
            _fixture.Create<int>() % BoardCoordinate.Max), _mineGeneratorMock.Object);
        var sut = new Player(notWinningTile);
        while (sut.IsAlive())
        {
            sut.MoveToTile(new Tile(_fixture.Create<BoardCoordinate>(), _mineGeneratorMock.Object));
        }
        
        sut.ToString().Should().Be($"Died with a score of {sut.Score}");
    }
    
    [Test]
    public void ToString_WhenPlayerHasWon_ReturnsExpected()
    {
        var winningTile = new Tile(new BoardCoordinate(BoardCoordinate.Max, BoardCoordinate.Max), _mineGeneratorMock.Object);
        var sut = new Player(winningTile);
        
        sut.ToString().Should().Be($"Won with a score of {sut.Score}");
    }
}