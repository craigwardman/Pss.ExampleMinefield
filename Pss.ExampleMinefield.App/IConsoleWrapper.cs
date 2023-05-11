namespace Pss.ExampleMinefield.App;

public interface IConsoleWrapper
{
    void WriteLine(string text);
    ConsoleKeyInfo ReadKey(bool intercept);
}