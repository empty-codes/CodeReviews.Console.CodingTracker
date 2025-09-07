using CodingTracker.empty_codes.Models;

namespace CodingTracker.empty_codes.Services
{
    public interface IGoalService
    {
        int TotalGoalHours { get; }
        double CurrentHours { get; }
        DateTime GoalDeadline { get; }
        double DailyTarget { get; }

        void SetGoal(List<CodingSession> sessions, int hours, DateTime deadline);
        double CalculateCurrentHours(List<CodingSession> sessions);
        double CalculateDailyTarget(int daysLeft);
    }
}
