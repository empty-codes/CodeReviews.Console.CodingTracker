using Spectre.Console.Rendering;

namespace CodingTracker.empty_codes.Services
{
    public interface IConsoleService
    {
        string? ReadLine();
        void WriteLine(string message);
        void Write(IRenderable renderable);
        void Clear();
        ConsoleKeyInfo ReadKey(bool intercept = false);
    }
}
