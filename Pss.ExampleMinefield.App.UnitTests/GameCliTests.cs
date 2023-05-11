using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using Moq;
using Pss.ExampleMinefield.Domain.Interfaces;
using Pss.ExampleMinefield.Domain.Model.Minefield;

namespace Pss.ExampleMinefield.App.UnitTests;

public class GameCliTests
{
    private readonly Mock<IConsoleWrapper> _consoleWrapperMock = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<IGame> _gameMock = new();

    public GameCliTests()
    {
        _fixture.Customize(new AutoMoqCustomization());
    }

    [SetUp]
    public void Setup()
    {
        _consoleWrapperMock.Reset();
        _consoleWrapperMock.Setup(c => c.ReadKey(It.IsAny<bool>()))
            .Returns(new ConsoleKeyInfo(' ', ConsoleKey.Escape, false, false, false));

        _gameMock.Reset();
    }

    [Test]
    public void Ctor_NullParam_Throws()
    {
        new GuardClauseAssertion(_fixture)
            .Verify(typeof(GameCli).GetConstructors());
    }

    [Test]
    public void Start_WhenCalled_PrintsWelcomeMessage()
    {
        GetDefaultSut().Start();

        _consoleWrapperMock.Verify(c =>
            c.WriteLine(
                "Welcome to the minefield game, use your arrow keys to navigate across the board without hitting a mine. Press Esc to exit."));
    }

    [TestCase(ConsoleKey.LeftArrow, Direction.Left)]
    [TestCase(ConsoleKey.UpArrow, Direction.Up)]
    [TestCase(ConsoleKey.RightArrow, Direction.Right)]
    [TestCase(ConsoleKey.DownArrow, Direction.Down)]
    public void Start_ArrowKeyPress_SendsCommandToGame(ConsoleKey consoleKey, Direction expectedDirection)
    {
        _consoleWrapperMock.SetupSequence(c => c.ReadKey(It.IsAny<bool>()))
            .Returns(new ConsoleKeyInfo(' ', consoleKey, false, false, false))
            .Returns(new ConsoleKeyInfo(' ', ConsoleKey.Escape, false, false, false));

        GetDefaultSut().Start();

        _gameMock.Verify(g => g.Move(expectedDirection));
    }

    [Test]
    public void Start_AnyKeyPress_ReportsStateOfPlay()
    {
        var stateOfPlay = _fixture.Create<string>();
        _gameMock.Setup(g => g.StateOfPlay()).Returns(stateOfPlay);
        _consoleWrapperMock.SetupSequence(c => c.ReadKey(It.IsAny<bool>()))
            .Returns(new ConsoleKeyInfo(' ', _fixture.Create<ConsoleKey>(), false, false, false))
            .Returns(new ConsoleKeyInfo(' ', ConsoleKey.Escape, false, false, false));

        GetDefaultSut().Start();

        _consoleWrapperMock.Verify(c => c.WriteLine(stateOfPlay));
    }

    private GameCli GetDefaultSut()
    {
        return new GameCli(_gameMock.Object, _consoleWrapperMock.Object);
    }
}