using CodingTracker.empty_codes.Models;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.empty_codes.Services
{
    internal class ReportService
    {
        public static List<CodingSession> FilterSessions(List<CodingSession> sessions, int filterChoice, int sortingChoice)
        {
            DateTime currentDate = DateTime.Now;

            switch (filterChoice)
            {
                case 1:
                    sessions = sessions.Where(s => s.StartTime.Date == currentDate.Date).ToList();
                    break;
                case 2:
                    sessions = sessions.Where(s => s.StartTime.Date >= currentDate.AddDays(-7)).ToList();
                    break;
                case 3:
                    sessions = sessions.Where(s => s.StartTime.Date >= currentDate.AddMonths(-1)).ToList();
                    break;
                case 4:
                    sessions = sessions.Where(s => s.StartTime.Date >= currentDate.AddYears(-1)).ToList();
                    break;
                default:
                    Console.WriteLine("Error: Unrecognized input.");
                    break;
            }

            switch (sortingChoice)
            {
                case 1:
                    sessions = sessions.OrderBy(s => s.StartTime).ToList();
                    break;
                case 2:
                    sessions = sessions.OrderByDescending(s => s.StartTime).ToList();
                    break;
                case 3:
                    sessions = sessions.OrderBy(s => s.Duration).ToList();
                    break;
                case 4:
                    sessions = sessions.OrderByDescending(s => s.Duration).ToList();
                    break;
                default:
                    Console.WriteLine("Error: Unrecognized input.");
                    break;
            }
            return sessions;
        }

        public static void GenerateReport(List<CodingSession> sessions)
        {
            if (sessions.Count == 0)
            {
                Console.WriteLine("No sessions found.");
                return;
            }

            TimeSpan totalDuration = sessions.Aggregate(TimeSpan.Zero, (sum, session) => sum.Add(session.Duration));

            TimeSpan averageDuration = TimeSpan.FromTicks(totalDuration.Ticks / sessions.Count);

            var longestSession = sessions.OrderByDescending(s => s.Duration).First();
            var shortestSession = sessions.OrderBy(s => s.Duration).First();

            Console.WriteLine($"Total Coding Time: {totalDuration}");
            Console.WriteLine($"Average Coding Time per Session: {averageDuration}");
            Console.WriteLine($"Number of Sessions: {sessions.Count}");
            Console.WriteLine($"Longest Session: {longestSession.Duration} on {longestSession.StartTime}");
            Console.WriteLine($"Shortest Session: {shortestSession.Duration} on {shortestSession.StartTime}");
        }
    }
}
