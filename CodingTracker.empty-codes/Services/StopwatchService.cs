using System.Diagnostics;
using Spectre.Console;

namespace CodingTracker.empty_codes.Services;

internal class StopwatchService
{
    public Stopwatch StopWatch { get; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsRunning { get; set; } = false;

    public StopwatchService()
    {
        StopWatch = new Stopwatch();
    }

    public void StartStopwatch()
    {
        if (IsRunning == false)
        {
            StartTime = DateTime.Now;
            StopWatch.Start();
            IsRunning = true;
            AnsiConsole.MarkupLine("[green]The stopwatch has started counting![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]The stopwatch is already running[/]");
        }
    }

    public void EndStopwatch()
    {
        if (IsRunning == true)
        {
            EndTime = DateTime.Now;
            StopWatch.Stop();
            IsRunning = false;
            AnsiConsole.MarkupLine("\n[green]The stopwatch has stopped![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("\n[yellow]The stopwatch has already ended[/]");
        }
    }

    public void CalculateDuration()
    {
        if (IsRunning == true)
        {
            AnsiConsole.MarkupLine("[red]Error: Stop the stopwatch first![/]");
        }
        else
        {
            Duration = EndTime - StartTime;
        }
    }
}