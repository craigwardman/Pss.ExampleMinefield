namespace Pss.ExampleMinefield.Domain.Model.Minefield;

public record BoardCoordinate(int X, int Y)
{
    private static readonly string[] ColumnNames = { "A", "B", "C", "D", "E", "F", "G", "H" };
    public const int Min = 0;
    public const int Max = 7;
    
    public int X { get; } =
        X is >= Min and <= Max ? X : throw new ArgumentOutOfRangeException(nameof(X), $"Supported co-ordinates range from {Min} to {Max}");

    public int Y { get; } =
        Y is >= Min and <= Max ? Y : throw new ArgumentOutOfRangeException(nameof(Y), $"Supported co-ordinates range from {Min} to {Max}");

    public override string ToString()
    {
        return $"{ColumnNames[X]}{Y+1}";
    }
}