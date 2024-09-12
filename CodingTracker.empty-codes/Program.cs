using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;
using Spectre.Console;
using CodingTracker.empty_codes.Controllers;
using CodingTracker.empty_codes.Views;

string? connectionString = ConfigurationManager.ConnectionStrings["CodingSessionDb"].ConnectionString;
string? dbPath = ConfigurationManager.AppSettings["DatabasePath"];
string? dateFormat = ConfigurationManager.AppSettings["DateFormat"];

if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(dbPath) || string.IsNullOrEmpty(dateFormat))
{
    AnsiConsole.MarkupLine("[red]Error: Missing configuration settings.[/]");
    return;
}

CreateDatabase(connectionString, dbPath);

CodingController controller = new CodingController(connectionString, dateFormat);
UserInput userInput = new UserInput(controller, dateFormat);

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



