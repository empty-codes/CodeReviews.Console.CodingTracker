using CodingTracker.empty_codes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.empty_codes.Services
{
    internal static class GoalService
    {
        public static int TotalGoalHours { get; set; }
        public static double CurrentHours { get; set; }
        public static DateTime GoalDeadline { get; set; }
        public static double DailyTarget { get; set; }

        public static void SetGoal(List<CodingSession> sessions, int hoursPerWeek, DateTime deadline)
        {
            TotalGoalHours = hoursPerWeek;
            GoalDeadline = deadline;
            CurrentHours = CalculateCurrentHours(sessions);
            DailyTarget = CalculateDailyTarget(sessions);

            Console.WriteLine($"You have completed {CurrentHours} hours of coding this week out of your {TotalGoalHours}-hour goal");
            Console.WriteLine($"You need to code {DailyTarget} hours per day to reach your weekly goal");
        }

        public static double CalculateCurrentHours(List<CodingSession> sessions)
        {
            double totalHours = sessions
                .Where(s => s.StartTime >= DateTime.Now.AddDays(-7)) 
                .Sum(s => s.Duration.TotalHours); 

            return totalHours;
        }

        public static double CalculateDailyTarget(List<CodingSession> sessions)
        {
            double hoursLeft = TotalGoalHours - CurrentHours;
            double daysLeft = (GoalDeadline - DateTime.Now).TotalDays;

            if(daysLeft <= 0)
            {
                Console.WriteLine("The deadline is today or it has passed.");
                return hoursLeft;
            }

            return hoursLeft / daysLeft;
        }

    }
}
