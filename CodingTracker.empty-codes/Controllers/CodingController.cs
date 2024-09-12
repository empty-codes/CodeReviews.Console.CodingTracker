using Microsoft.Data.Sqlite;
using Dapper;
using Spectre.Console;
using CodingTracker.empty_codes.Models;

namespace CodingTracker.empty_codes.Controllers;

internal class CodingController
{
    public string? ConnectionString { get; }
    public string DateFormat { get; }

    public CodingController(string connectionString, string dateFormat)
    {
        ConnectionString = connectionString;
        DateFormat = dateFormat;
    }

    public void InsertSession(CodingSession session)
    {
        using var conn = new SqliteConnection(ConnectionString);
        string insertQuery = "INSERT INTO CodingSessions(StartTime, EndTime, Duration) VALUES(@StartTime, @EndTime, @Duration)";
        try
        {
            var parameters = new
            {
                StartTime = session.StartTime.ToString(DateFormat),
                EndTime = session.EndTime.ToString(DateFormat),
                Duration = session.Duration.ToString(@"hh\:mm\:ss")
            };
            conn.Execute(insertQuery, parameters);

            string getIdQuery = "SELECT last_insert_rowid();";
            session.Id = conn.ExecuteScalar<int>(getIdQuery);

            AnsiConsole.MarkupLine($"[green]Session successfully added. (Session Id: {session.Id})[/]");
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to insert your session\n - Details: {e.Message}[/]");
        }
    }

    public List<CodingSession> ViewAllSessions()
    {
        var sessions = new List<CodingSession>();
        using var conn = new SqliteConnection(ConnectionString);
        string readQuery = "SELECT * FROM CodingSessions";
        try
        {
            var rawSessions = conn.Query(readQuery).ToList();

            foreach (var rawSession in rawSessions)
            {
                CodingSession session = new CodingSession();
                session.Id = (int)rawSession.Id;
                session.StartTime = DateTime.ParseExact(rawSession.StartTime, DateFormat, null);
                session.EndTime = DateTime.ParseExact(rawSession.EndTime, DateFormat, null);
                session.Duration = TimeSpan.Parse(rawSession.Duration);
                sessions.Add(session);
            }
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to access your sessions\n - Details: {e.Message}[/]");
        }
        return sessions;
    }

    public void UpdateSession(CodingSession session)
    {
        using var conn = new SqliteConnection(ConnectionString);
        string updateQuery = "UPDATE CodingSessions SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";
        try
        {
            var parameters = new
            {
                StartTime = session.StartTime.ToString(DateFormat),
                EndTime = session.EndTime.ToString(DateFormat),
                Duration = session.Duration.ToString(@"hh\:mm\:ss"),
                Id = session.Id
            };
            int result = conn.Execute(updateQuery, parameters);

            if (result == 0)
            {
                AnsiConsole.MarkupLine($"[yellow]No session found with the provided Id: {session.Id}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Session with Id: {session.Id} successfully updated.[/]");
            }
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to update your session\n - Details: {e.Message}[/]");
        }
    }

    public void DeleteSession(CodingSession session)
    {
        using var conn = new SqliteConnection(ConnectionString);
        string deleteQuery = "DELETE FROM CodingSessions WHERE Id = @Id";
        try
        {
            int result = conn.Execute(deleteQuery, session);

            if (result == 0)
            {
                AnsiConsole.MarkupLine($"[yellow]No session found with the provided Id: {session.Id}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Session with Id: {session.Id} successfully deleted.[/]");
            }
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to delete your session\n - Details: {e.Message}[/]");
        }
    }
}