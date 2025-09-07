using System.Diagnostics;
using Spectre.Console;

namespace CodingTracker.empty_codes.Services;

internal class StopwatchService : IStopwatchService
{
    private readonly IConsoleService Console;
    private Stopwatch StopWatch = new Stopwatch();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsRunning { get; set; }

    public StopwatchService(IConsoleService console)
    {
        Console = console;
    }

    public void StartStopwatch()
    {
        if (IsRunning == false)
        {
            StartTime = DateTime.Now;
            StopWatch.Start();
            IsRunning = true;
            Console.WriteLine("[green]The stopwatch has started counting![/]");
        }
        else
        {
            Console.WriteLine("[yellow]The stopwatch is already running[/]");
        }
    }

    public void EndStopwatch()
    {
        if (IsRunning == true)
        {
            EndTime = DateTime.Now;
            StopWatch.Stop();
            IsRunning = false;
            Console.WriteLine("\n[green]The stopwatch has stopped![/]");
        }
        else
        {
            Console.WriteLine("\n[yellow]The stopwatch has already ended[/]");
        }
    }

    public void CalculateDuration()
    {
        if (IsRunning == true)
        {
            Console.WriteLine("[red]Error: Stop the stopwatch first![/]");
        }
        else
        {
            Duration = EndTime - StartTime;
        }
    }
}