using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker.empty_codes.Controllers;
using CodingTracker.empty_codes.Views;

string? connectionString = ConfigurationManager.ConnectionStrings["CodingSessionDb"].ConnectionString;
string? dbPath = ConfigurationManager.AppSettings["DatabasePath"];
string? dateFormat = ConfigurationManager.AppSettings["DateFormat"];

if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(dbPath) || string.IsNullOrEmpty(dateFormat))
{
    Console.WriteLine("Error: Missing configuration settings.");
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
        Console.WriteLine($"Error occured while trying to create the database Table\n - Details: {e.Message}");
    }
    Console.WriteLine($"Database file {dbPath} successfully created.\n");
}



