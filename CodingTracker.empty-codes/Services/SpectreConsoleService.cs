using Spectre.Console;
using Spectre.Console.Rendering;

namespace CodingTracker.empty_codes.Services
{
    internal class SpectreConsoleService : IConsoleService  
    {
        public string? ReadLine() => Console.ReadLine();
        public void WriteLine(string message) => AnsiConsole.MarkupLine(message);
        public void Write(IRenderable renderable) => AnsiConsole.Write(renderable);
        public void Clear() => AnsiConsole.Clear();
        public ConsoleKeyInfo ReadKey(bool intercept = false) => Console.ReadKey(intercept);
    }
}
