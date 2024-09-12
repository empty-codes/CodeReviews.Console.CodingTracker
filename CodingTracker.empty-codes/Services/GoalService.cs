using CodingTracker.empty_codes.Models;
using Spectre.Console;
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

        public static void SetGoal(List<CodingSession> sessions, int hours, DateTime deadline)
        {
            TotalGoalHours = hours;
            GoalDeadline = deadline;
            CurrentHours = CalculateCurrentHours(sessions);
       
            int daysLeft = (deadline - DateTime.Now).Days;
            if(daysLeft <= 0)
            {
                AnsiConsole.MarkupLine("[red]The deadline is today or has passed. You can't set this goal.[/]\n");
                return;
            }
            DailyTarget = CalculateDailyTarget(daysLeft);

            Console.Clear();
            AnsiConsole.MarkupLine("[underline]GOAL STATS[/]\n");
            AnsiConsole.MarkupLine($"[green]You have completed {Math.Round(CurrentHours, 2)} hours of coding this week out of your {TotalGoalHours}-hour goal[/]");
            if (CurrentHours >= TotalGoalHours)
            {
                AnsiConsole.MarkupLine("[green]Congratulations! You have reached your goal![/]\n");
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]You need to code {Math.Round(DailyTarget, 2)} hours per day to reach your weekly goal[/]\n");
            }

            double completedPercentage = (CurrentHours / TotalGoalHours) * 100;
            double remainingPercentage = 100 - completedPercentage;

            AnsiConsole.Write(new BreakdownChart()
                .Width(60)
                .AddItem("Completed Hours", completedPercentage, Color.Green)
                .AddItem("Remaining Hours", remainingPercentage, Color.Red));
        }

        public static double CalculateCurrentHours(List<CodingSession> sessions)
        {
            return sessions.Sum(s => s.Duration.TotalHours);
        }

        public static double CalculateDailyTarget(int daysLeft)
        {
            double hoursLeft = TotalGoalHours - CurrentHours;
            return hoursLeft / daysLeft;
        }

    }
}
