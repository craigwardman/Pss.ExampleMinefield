namespace Pss.ExampleMinefield.App;

internal class DotnetConsoleWrapper : IConsoleWrapper
{
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }

    public ConsoleKeyInfo ReadKey(bool intercept)
    {
        return Console.ReadKey(intercept);
    }
}