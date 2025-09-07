using CodingTracker.empty_codes.Models;
using System.Diagnostics;

namespace CodingTracker.empty_codes.Services
{
    public interface IStopwatchService
    {
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        TimeSpan Duration { get; set; }
        bool IsRunning { get; set; }

        void StartStopwatch();
        void EndStopwatch();
        void CalculateDuration();
    }
}
