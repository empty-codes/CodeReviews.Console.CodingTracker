using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CodingTracker.empty_codes.Services
{
    internal class StopwatchService
    {
        public Stopwatch? StopWatch { get;} 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsRunning { get; set; } = false;

        public StopwatchService()
        {
            StopWatch = new Stopwatch();
        }

        public void StartStopwatch()
        {
            if(IsRunning == false)
            {
                StartTime = DateTime.Now;
                StopWatch.Start();
                Console.Write("The stopwatch has started counting!");
            }
            else
            {
                Console.Write("The stopwatch is already running");
            }
        }

        public void EndStopwatch()
        {
            if(IsRunning == true)
            {
                EndTime = DateTime.Now;
                StopWatch.Stop();
                Console.Write("The stopwatch has stopped!");
            }
            else
            {
                Console.Write("The stopwatch has already ended");
            }
        }

        public void CalculateDuration()
        {
            if (IsRunning == true)
            {
                Console.WriteLine("Error: Stop the stopwatch first!");
            }
            else
            {
                Duration = EndTime - StartTime;
            }
        }
    }
}
