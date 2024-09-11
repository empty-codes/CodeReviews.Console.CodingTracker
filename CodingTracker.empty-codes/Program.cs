using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Data.Sqlite;
using Dapper; 

string? connectionString = ConfigurationManager.ConnectionStrings["CodingSessionDb"].ConnectionString;

string? dbPath = ConfigurationManager.AppSettings["DatabasePath"];
string? dateFormat = ConfigurationManager.AppSettings["DateFormat"];

CreateDatabase(connectionString);

void CreateDatabase(string connectionString)
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



