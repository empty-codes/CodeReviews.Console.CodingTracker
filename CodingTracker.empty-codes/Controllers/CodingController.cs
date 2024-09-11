using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker.empty_codes.Models;

namespace CodingTracker.empty_codes.Controllers
{
    internal class CodingController
    {
        public string ConnectionString;

        public CodingController(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void InsertSession(CodingSession session)
        {
            using var conn = new SqliteConnection(ConnectionString);
            string insertQuery = "INSERT INTO CodingSessions(StartTime, EndTime, Duration) VALUES(@start, @end, @duration)";
            try
            {
                conn.Execute(insertQuery, session);

                string getIdQuery = "SELECT last_insert_rowid();";
                session.Id = conn.ExecuteScalar<int>(getIdQuery);

                Console.WriteLine($"Session successfully added. (Session Id: {session.Id})\n");
            }
            catch (SqliteException e)
            {
                Console.WriteLine($"Error occured while trying to insert your session\n - Details: {e.Message}");
            }
        }

        public List<CodingSession> ViewAllSessions()
        {
            var sessions = new List<CodingSession>;
            using var conn = new SqliteConnection(ConnectionString);
            string readQuery = "SELECT * FROM CodingSessions";
            try
            {
                sessions = conn.Query<CodingSession>(readQuery).ToList();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to access your sessions\n - Details: " + e.Message);
            }
            return sessions;
        }

        public void UpdateSession(CodingSession session)
        {
            using var conn = new SqliteConnection(ConnectionString);
            string updateQuery = "UPDATE CodingSessions SET StartTime = @start, EndTime = @end, Duration = @duration WHERE Id = @id";
            try
            {
                int result = conn.Execute(updateQuery, session);

                if(result == 0)
                {
                    Console.WriteLine($"No session found with the provided Id: {session.Id}\n");
                }
                else
                {
                    Console.WriteLine($"Session with Id: {session.Id} successfully updated.\n");
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine($"Error occured while trying to update your session\n - Details: {e.Message}");
            }
        }

        public void DeleteSession(CodingSession session)
        {
            using var conn = new SqliteConnection(ConnectionString);
            string deleteQuery = "DELETE FROM CodingSessions WHERE Id = @id";
            try
            {
                int result = conn.Execute(deleteQuery, session);

                if (result == 0)
                {
                    Console.WriteLine($"No session found with the provided Id: {session.Id}\n");
                }
                else
                {
                    Console.WriteLine($"Session with Id: {session.Id} successfully deleted.\n");
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine($"Error occured while trying to delete your session\n - Details: {e.Message}");
            }
        }
    }
}
