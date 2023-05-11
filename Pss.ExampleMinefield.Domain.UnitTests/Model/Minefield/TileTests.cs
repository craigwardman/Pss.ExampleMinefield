using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Pss.ExampleMinefield.Domain.Model.Minefield;
using Pss.ExampleMinefield.Domain.Services.Minefield;

namespace Pss.ExampleMinefield.Domain.UnitTests.Model.Minefield;

[TestFixture]
public class TileTests
{
    [SetUp]
    public void SetUp()
    {
        _mineGeneratorMock.Reset();
    }

    private readonly Fixture _fixture = new();
    private readonly Mock<IMineGenerator> _mineGeneratorMock = new();

    public TileTests()
    {
        _fixture.Customize(new AutoMoqCustomization());
        _fixture.Customize<BoardCoordinate>(composer => composer.FromFactory(() => new BoardCoordinate(
            _fixture.Create<int>() % 8,
            _fixture.Create<int>() % 8)));
    }

    [Test]
    public void Ctor_NullParam_Throws()
    {
        new GuardClauseAssertion(_fixture)
            .Verify(typeof(Tile).GetConstructors());
    }

    [TestCase(true)]
    [TestCase(false)]
    public void Props_WhenCalled_AreExpected(bool shouldHaveMine)
    {
        var coordinate = _fixture.Create<BoardCoordinate>();
        _mineGeneratorMock.Setup(g => g.ShouldLayMine(coordinate))
            .Returns(shouldHaveMine);

        var sut = new Tile(coordinate, _mineGeneratorMock.Object);

        using (new AssertionScope())
        {
            sut.HasMine.Should().Be(shouldHaveMine);
            sut.Coordinate.Should().Be(coordinate);
        }
    }
}