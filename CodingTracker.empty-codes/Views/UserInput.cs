using CodingTracker.empty_codes.Models;
using CodingTracker.empty_codes.Controllers;
using CodingTracker.empty_codes.Services;
using Spectre.Console;

namespace CodingTracker.empty_codes.Views;

internal class UserInput
{
    public CodingController Controller { get; }
    public string DateFormat { get; }
    public UserInput(CodingController controller, string dateFormat)
    {
        Controller = controller;
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

            int choice = ValidationService.IsMenuChoiceValid(1, 8);

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
                    ReportService.GenerateReport(Controller.ViewAllSessions());
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

    public void UseStopwatch()
    {
        StopwatchService stopwatch = new StopwatchService();
        stopwatch.StartStopwatch();

        AnsiConsole.MarkupLine("[bold yellow]Press any key to stop the stopwatch[/]");
        Console.ReadKey();

        stopwatch.EndStopwatch();
        stopwatch.CalculateDuration();

        CodingSession codingSession = new CodingSession();
        codingSession.StartTime = stopwatch.StartTime;
        codingSession.EndTime = stopwatch.EndTime;
        codingSession.Duration = stopwatch.Duration;
        Controller.InsertSession(codingSession);
    }

    public void SetCodingGoals()
    {
        AnsiConsole.Markup("[bold]How many coding hours do you want to achieve? [/]");
        int hours = ValidationService.IsMenuChoiceValid(0, 100000);

        AnsiConsole.Markup("[bold]What is your target date to have completed this goal? (yyyy-MM-dd HH:mm)[/] ");
        DateTime deadline = ValidationService.IsDateValid();

        GoalService.SetGoal(Controller.ViewAllSessions(), hours, deadline);
    }

    public void ViewSessions()
    {
        var sessions = Controller.ViewAllSessions();
        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold]\nChoose 1 to View All Sessions (default) or 2 to View Records with custom filters:[/]");
            int choice = ValidationService.IsMenuChoiceValid(1, 2);

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
                int filterChoice = ValidationService.IsMenuChoiceValid(1, 4);

                AnsiConsole.MarkupLine("[bold]\nChoose sorting order:[/]");
                AnsiConsole.MarkupLine("[bold]1[/] - Ascending Order of Date");
                AnsiConsole.MarkupLine("[bold]2[/] - Descending Order of Date");
                AnsiConsole.MarkupLine("[bold]3[/] - Ascending Order of Duration");
                AnsiConsole.MarkupLine("[bold]4[/] - Descending Order of Duration");
                int sortingChoice = ValidationService.IsMenuChoiceValid(1, 4);

                var filteredSessions = ReportService.FilterSessions(sessions, filterChoice, sortingChoice);

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

    public void AddSession()
    {
        AnsiConsole.Markup("[bold]Enter the start time using the 24H format (yyyy-MM-dd HH:mm): [/]");
        DateTime startTime = ValidationService.IsDateValid();

        AnsiConsole.Markup("[bold]Enter the end time using the 24H format (yyyy-MM-dd HH:mm): [/]");
        DateTime endTime = ValidationService.IsDateValid();

        if (ValidationService.IsEndDateValid(startTime, endTime))
        {
            CodingSession codingSession = new CodingSession();
            codingSession.StartTime = startTime;
            codingSession.EndTime = endTime;

            codingSession.CalculateDuration();
            Controller.InsertSession(codingSession);
        }
    }

    public void UpdateSession()
    {
        AnsiConsole.Markup("[bold]Enter the Id of the session you wish to update: [/]");
        int.TryParse(Console.ReadLine(), out int updateId);

        CodingSession codingSession = new CodingSession();
        codingSession.Id = updateId;

        AnsiConsole.Markup("[bold]Enter the new start time using the 24H format (yyyy-MM-dd HH:mm): [/]");
        DateTime startTime = ValidationService.IsDateValid();

        AnsiConsole.Markup("[bold]Enter the new end time using the 24H format (yyyy-MM-dd HH:mm): [/]");
        DateTime endTime = ValidationService.IsDateValid();

        if (ValidationService.IsEndDateValid(startTime, endTime))
        {
            codingSession.StartTime = startTime;
            codingSession.EndTime = endTime;

            codingSession.CalculateDuration();
        }
        Controller.UpdateSession(codingSession);
    }

    public void DeleteSession()
    {
        AnsiConsole.Markup("[bold]Enter the Id of the session you wish to delete: [/]");
        int.TryParse(Console.ReadLine(), out int deleteId);
        CodingSession codingSession = new CodingSession();
        codingSession.Id = deleteId;
        Controller.DeleteSession(codingSession);
    }
}