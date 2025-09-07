using Microsoft.Data.Sqlite;
using Dapper;
using Spectre.Console;
using CodingTracker.empty_codes.Controllers;
using CodingTracker.empty_codes.Views;
using CodingTracker.empty_codes.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

string? connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CodingSessionDb"].ConnectionString;
string? dbPath = System.Configuration.ConfigurationManager.AppSettings["DatabasePath"];
string? dateFormat = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];

if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(dbPath) || string.IsNullOrEmpty(dateFormat))
{
    AnsiConsole.MarkupLine("[red]Error: Missing configuration settings.[/]");
    return;
}

CreateDatabase(connectionString, dbPath);

//DI
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<IGoalService, GoalService>();
        services.AddSingleton<IReportService, ReportService>();
        services.AddSingleton<IStopwatchService, StopwatchService>();
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IConsoleService, SpectreConsoleService>();

        services.AddTransient<ICodingController>(sp =>
            new CodingController(connectionString, dateFormat));

        services.AddTransient<IUserInput>(sp =>
            new UserInput(
                sp.GetRequiredService<ICodingController>(),
                sp.GetRequiredService<IValidationService>(),
                sp.GetRequiredService<IReportService>(),
                sp.GetRequiredService<IGoalService>(),
                sp.GetRequiredService<IStopwatchService>(),
                sp.GetRequiredService<IConsoleService>(),
                dateFormat));
    })
    .Build();

var userInput = host.Services.GetRequiredService<IUserInput>();
userInput.GetUserInput();

static void CreateDatabase(string connectionString, string dbPath)
{
    using var conn = new SqliteConnection(connectionString);

    var createTableQuery = @"
            CREATE TABLE IF NOT EXISTS CodingSessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                Duration TEXT NOT NULL
            );";
    try
    {
        conn.Execute(createTableQuery);
    }
    catch (SqliteException e)
    {
        AnsiConsole.MarkupLine($"[red]Error occurred while trying to create the database Table\n - Details: {e.Message}[/]");
    }
    AnsiConsole.MarkupLine($"[green]Database file {dbPath} successfully created.[/] [green]The database is ready to use.[/]");
    Console.Clear();
}