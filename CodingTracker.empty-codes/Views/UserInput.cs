using CodingTracker.empty_codes.Models;
using CodingTracker.empty_codes.Controllers;
using CodingTracker.empty_codes.Services;
using Spectre.Console;

namespace CodingTracker.empty_codes.Views;

internal class UserInput : IUserInput
{
    private readonly ICodingController Controller;
    private readonly IValidationService _validationService;
    private readonly IReportService _reportService;
    private readonly IGoalService _goalService;
    private readonly IStopwatchService _stopwatchService;
    private string DateFormat { get; }
    public UserInput(
        ICodingController controller,
        IValidationService validationService,
        IReportService reportService,
        IGoalService goalService,
        IStopwatchService stopwatchService,
        string dateFormat)
    {
        Controller = controller;
        _validationService = validationService;
        _reportService = reportService;
        _goalService = goalService;
        _stopwatchService = stopwatchService;
        DateFormat = dateFormat;
    }

    public void GetUserInput()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[underline green]MAIN MENU[/]");
            AnsiConsole.MarkupLine("[bold]Welcome to empty's Coding Tracker :)[/]");
            AnsiConsole.MarkupLine("Choose an option using the numbers below:\n");
            AnsiConsole.MarkupLine("[bold]1[/] - Track Coding time in real-time");
            AnsiConsole.MarkupLine("[bold]2[/] - Set Coding Goals");
            AnsiConsole.MarkupLine("[bold]3[/] - View all Sessions Recorded");
            AnsiConsole.MarkupLine("[bold]4[/] - Insert a Session");
            AnsiConsole.MarkupLine("[bold]5[/] - Update a Session");
            AnsiConsole.MarkupLine("[bold]6[/] - Delete a Session");
            AnsiConsole.MarkupLine("[bold]7[/] - View a Tailored Report of your Sessions");
            AnsiConsole.MarkupLine("[bold]8[/] - Exit this Application");

            int choice = _validationService.IsMenuChoiceValid(1, 8);

            switch (choice)
            {
                case 1:
                    UseStopwatch();
                    break;
                case 2:
                    SetCodingGoals();
                    break;
                case 3:
                    ViewSessions();
                    break;
                case 4:
                    AddSession();
                    break;
                case 5:
                    UpdateSession();
                    break;
                case 6:
                    DeleteSession();
                    break;
                case 7:
                    _reportService.GenerateReport(Controller.ViewAllSessions());
                    break;
                case 8:
                    return;
                default:
                    AnsiConsole.MarkupLine("[red]Error: Unrecognized input.[/]");
                    break;
            }
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void UseStopwatch()
    {
        _stopwatchService.StartStopwatch();

        AnsiConsole.MarkupLine("[bold yellow]Press any key to stop the stopwatch[/]");
        Console.ReadKey();

        _stopwatchService.EndStopwatch();
        _stopwatchService.CalculateDuration();

        var codingSession = new CodingSession
        {
            StartTime = _stopwatchService.StartTime,
            EndTime = _stopwatchService.EndTime,
            Duration = _stopwatchService.Duration
        };

        Controller.InsertSession(codingSession);
    }

    private void SetCodingGoals()
    {
        AnsiConsole.Markup("[bold]How many coding hours do you want to achieve? [/]");
        int hours = _validationService.IsMenuChoiceValid(0, 100000);

        AnsiConsole.Markup("[bold]What is your target date to have completed this goal? (yyyy-MM-dd HH:mm)[/] ");
        string? targetInput = Console.ReadLine();
        DateTime deadline = _validationService.IsDateValid(targetInput);

        _goalService.SetGoal(Controller.ViewAllSessions(), hours, deadline);
    }

    private void ViewSessions()
    {
        var sessions = Controller.ViewAllSessions();
        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold]\nChoose 1 to View All Sessions (default) or 2 to View Records with custom filters:[/]");
            int choice = _validationService.IsMenuChoiceValid(1, 2);

            if (choice == 1)
            {
                var table = new Table();
                table.Title = new TableTitle("All Coding Sessions", Style.Parse("bold yellow"));
                table.AddColumn("[bold]Id[/]");
                table.AddColumn("[bold]Start[/]");
                table.AddColumn("[bold]End[/]");
                table.AddColumn("[bold]Duration[/]");

                foreach (var session in sessions)
                {
                    table.AddRow(
                           session.Id.ToString(),
                           session.StartTime.ToString(DateFormat),
                           session.EndTime.ToString(DateFormat),
                           session.Duration.ToString(@"hh\:mm\:ss")
                       );
                }
                Console.Clear();
                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[bold]\nChoose from the filtering options below:[/]");
                AnsiConsole.MarkupLine("[bold]1[/] - Filter Sessions by Today");
                AnsiConsole.MarkupLine("[bold]2[/] - Filter Sessions by Last Week");
                AnsiConsole.MarkupLine("[bold]3[/] - Filter Sessions by Last Month");
                AnsiConsole.MarkupLine("[bold]4[/] - Filter Sessions by Last Year");
                int filterChoice = _validationService.IsMenuChoiceValid(1, 4);

                AnsiConsole.MarkupLine("[bold]\nChoose sorting order:[/]");
                AnsiConsole.MarkupLine("[bold]1[/] - Ascending Order of Date");
                AnsiConsole.MarkupLine("[bold]2[/] - Descending Order of Date");
                AnsiConsole.MarkupLine("[bold]3[/] - Ascending Order of Duration");
                AnsiConsole.MarkupLine("[bold]4[/] - Descending Order of Duration");
                int sortingChoice = _validationService.IsMenuChoiceValid(1, 4);

                var filteredSessions = _reportService.FilterSessions(sessions, filterChoice, sortingChoice);

                var table = new Table();
                table.Title = new TableTitle("All Coding Sessions", Style.Parse("bold yellow"));
                table.AddColumn("[bold]Id[/]");
                table.AddColumn("[bold]Start[/]");
                table.AddColumn("[bold]End[/]");
                table.AddColumn("[bold]Duration[/]");

                foreach (var session in filteredSessions)
                {
                    table.AddRow(
                           session.Id.ToString(),
                           session.StartTime.ToString(DateFormat),
                           session.EndTime.ToString(DateFormat),
                           session.Duration.ToString(@"hh\:mm\:ss")
                       );
                }
                Console.Clear();
                AnsiConsole.Write(table);
            }
        }
    }

    private void AddSession()
    {
        AnsiConsole.Markup("[bold]Enter the start time using the 24H format (yyyy-MM-dd HH:mm): [/]");
        string? startInput = Console.ReadLine();
        DateTime startTime = _validationService.IsDateValid(startInput);

        AnsiConsole.Markup("[bold]Enter the end time using the 24H format (yyyy-MM-dd HH:mm): [/]");
        string? endInput = Console.ReadLine();
        DateTime endTime = _validationService.IsDateValid(endInput);

        if (_validationService.IsEndDateValid(startTime, endTime))
        {
            CodingSession codingSession = new CodingSession();
            codingSession.StartTime = startTime;
            codingSession.EndTime = endTime;

            codingSession.CalculateDuration();
            Controller.InsertSession(codingSession);
        }
    }

    private void UpdateSession()
    {
        var sessions = Controller.ViewAllSessions();
        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found![/]");
        }
        else
        {
            var table = new Table();
            table.Title = new TableTitle("All Coding Sessions", Style.Parse("bold yellow"));
            table.AddColumn("[bold]Id[/]");
            table.AddColumn("[bold]Start[/]");
            table.AddColumn("[bold]End[/]");
            table.AddColumn("[bold]Duration[/]");

            foreach (var session in sessions)
            {
                table.AddRow(
                       session.Id.ToString(),
                       session.StartTime.ToString(DateFormat),
                       session.EndTime.ToString(DateFormat),
                       session.Duration.ToString(@"hh\:mm\:ss")
                   );
            }
            Console.Clear();
            AnsiConsole.Write(table);
        }

        AnsiConsole.Markup("[bold]Enter the Id of the session you wish to update: [/]");
        int.TryParse(Console.ReadLine(), out int updateId);

        var existingSession = sessions.FirstOrDefault(s => s.Id == updateId);
        if (existingSession == null)
        {
            AnsiConsole.MarkupLine("[red]No session found with the given Id.[/]");
            return;
        }

        CodingSession codingSession = new CodingSession();
        codingSession.Id = updateId;
        DateTime startTime, endTime;

        AnsiConsole.Write($"Enter the new start time using the 24H format (yyyy-MM-dd HH:mm) or press Enter to keep {existingSession.StartTime.ToString(DateFormat)}: ");
        string? startInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(startInput))
        {
            startTime = existingSession.StartTime;
        }
        else
        {
            startTime = _validationService.IsDateValid(startInput);
        }

        AnsiConsole.Write($"Enter the new end time using the 24H format (yyyy-MM-dd HH:mm) or press Enter to keep {existingSession.EndTime.ToString(DateFormat)}: ");
        string? endInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(endInput))
        {
            endTime = existingSession.EndTime;
        }
        else
        {
            endTime = _validationService.IsDateValid(endInput);
        }

        if (startTime == existingSession.StartTime && endTime == existingSession.EndTime)
        {
            AnsiConsole.MarkupLine("[red]Entered Start Time and End Time is the same as existing, No changes have been made.[/]");
        }
        else
        {
            if (_validationService.IsEndDateValid(startTime, endTime))
            {
                codingSession.StartTime = startTime;
                codingSession.EndTime = endTime;

                codingSession.CalculateDuration();
            }
            Controller.UpdateSession(codingSession);
        }
        
    }

    private void DeleteSession()
    {
        var sessions = Controller.ViewAllSessions();
        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found![/]");
        }
        else
        {
            var table = new Table();
            table.Title = new TableTitle("All Coding Sessions", Style.Parse("bold yellow"));
            table.AddColumn("[bold]Id[/]");
            table.AddColumn("[bold]Start[/]");
            table.AddColumn("[bold]End[/]");
            table.AddColumn("[bold]Duration[/]");

            foreach (var session in sessions)
            {
                table.AddRow(
                       session.Id.ToString(),
                       session.StartTime.ToString(DateFormat),
                       session.EndTime.ToString(DateFormat),
                       session.Duration.ToString(@"hh\:mm\:ss")
                   );
            }
            Console.Clear();
            AnsiConsole.Write(table);
        }
        AnsiConsole.Markup("[bold]Enter the Id of the session you wish to delete: [/]");
        int.TryParse(Console.ReadLine(), out int deleteId);
        CodingSession codingSession = new CodingSession();
        codingSession.Id = deleteId;
        Controller.DeleteSession(codingSession);
    }
}