using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingTracker.empty_codes.Models;
using CodingTracker.empty_codes.Controllers;
using CodingTracker.empty_codes.Services;
using System.Configuration;

namespace CodingTracker.empty_codes.Views
{
    internal class UserInput
    {
        public CodingController Controller { get; }
        public string DateFormat { get; }
        public UserInput(CodingController controller, string dateFormat)
        {
            Controller = controller;
            DateFormat = dateFormat;
        }

        public void GetUserInput()
        {
            while(true)
            {
                
                Console.WriteLine("________________________");
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("________________________");
                Console.WriteLine("Welcome to empty's Coding Tracker :)");
                Console.WriteLine("Choose an option using the numbers below:\n");
                Console.WriteLine("1 to Track Coding time in real-time");
                Console.WriteLine("2 to Set Coding Goals");
                Console.WriteLine("3 to View all Sessions Recorded");
                Console.WriteLine("4 to Insert a Session");
                Console.WriteLine("5 to Update a Session");
                Console.WriteLine("6 to Delete a Session");
                Console.WriteLine("7 to View a Tailored Report of your Sessions");
                Console.WriteLine("8 to Exit this Application");
                Console.WriteLine("________________________\n");
                int choice = ValidationService.IsMenuChoiceValid(1, 8);

                switch (choice)
                {
                    case 1:
                        UseStopwatch();
                        break;
                    case 2:
                        SetCodingGoals();
                        break;
                    case 3:
                        ViewSessions();
                        break;
                    case 4:
                        AddSession();
                        break;
                    case 5:
                        UpdateSession();
                        break;
                    case 6:
                        DeleteSession();
                        break;
                    case 7:
                        ReportService.GenerateReport(Controller.ViewAllSessions());
                        break;
                    case 8:
                        return;
                    default:
                        Console.WriteLine("Error: Unrecognized input.");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }


        }


        public void UseStopwatch()
        {
            StopwatchService stopwatch = new StopwatchService();
            stopwatch.StartStopwatch();

            Console.Write("Press any key to stop the stopwatch");
            Console.ReadKey();

            stopwatch.EndStopwatch();
            stopwatch.CalculateDuration();

            CodingSession codingSession = new CodingSession();
            codingSession.StartTime = stopwatch.StartTime;
            codingSession.EndTime = stopwatch.EndTime;
            codingSession.Duration = stopwatch.Duration;
            Controller.InsertSession(codingSession);
        }
    
        public void SetCodingGoals()
        {
            Console.Write("How many coding hours do you want to achieve per week? ");
            int hoursPerWeek = ValidationService.IsMenuChoiceValid(0, 168);

            Console.Write("What is your target date to have completed this goal? (yyyy-MM-dd HH:mm (24h format))");
            DateTime deadline = ValidationService.IsDateValid();

            CodingSession codingSession = new CodingSession();

            GoalService.SetGoal(Controller.ViewAllSessions(), hoursPerWeek, deadline);
        }

        public void ViewSessions()
        {
            Console.WriteLine("Choose 1 to View All Sessions (default) or to 2 to View Records with custom filters");
            int choice = ValidationService.IsMenuChoiceValid(1, 2);

            if(choice == 1)
            {
                var sessions = Controller.ViewAllSessions();

                foreach (var session in sessions)
                {
                    Console.WriteLine($"Id: {session.Id}, Start: {session.StartTime}, End: {session.EndTime}, Duration: {session.Duration}");
                }
            }
            else
            {
                Console.WriteLine("Choose from the filtering options below: ");
                Console.WriteLine("1 to Filter Sessions by Today");
                Console.WriteLine("2 to Filter Sessions by Last Week");
                Console.WriteLine("3 to Filter Sessions by Last Month");
                Console.WriteLine("4 to Filter Sessions by Last Year");
                int filterChoice = ValidationService.IsMenuChoiceValid(1, 4);

                Console.WriteLine("Choose sorting order: ");
                Console.WriteLine("1 to Sort by Ascending Order of Date");
                Console.WriteLine("2 to Sort by Descending Order of Date");
                Console.WriteLine("3 to Sort by Ascending Order of Duration");
                Console.WriteLine("4 to Sort by Descending Order of Duration");
                int sortingChoice = ValidationService.IsMenuChoiceValid(1, 4);

                var filteredSessions = ReportService.FilterSessions(Controller.ViewAllSessions(), filterChoice, sortingChoice);

                foreach (var session in filteredSessions)
                {
                    Console.WriteLine($"Id: {session.Id}, Start: {session.StartTime}, End: {session.EndTime}, Duration: {session.Duration}");
                }
            }
            

        }

        public void AddSession()
        {
            Console.Write("Enter the start time using the 24H format (yyyy-MM-dd HH:mm): ");
            DateTime startTime = ValidationService.IsDateValid();

            Console.Write("Enter the end time using the 24H format (yyyy-MM-dd HH:mm):4 ");
            DateTime endTime = ValidationService.IsDateValid();

            if(ValidationService.IsEndDateValid(startTime, endTime))
            {
                CodingSession codingSession = new CodingSession();
                codingSession.StartTime = startTime;
                codingSession.EndTime = endTime;

                codingSession.CalculateDuration();
                Controller.InsertSession(codingSession);            }
        }

        public void UpdateSession()
        {
            Console.Write("Enter the Id of the session you wish to update:");
            int.TryParse(Console.ReadLine(), out int updateId);
            
            CodingSession codingSession = new CodingSession();
            codingSession.Id = updateId;

            Console.Write("Enter the new start time using the 24H format (yyyy-MM-dd HH:mm): ");
            DateTime startTime = ValidationService.IsDateValid();

            Console.Write("Enter the new end time using the 24H format (yyyy-MM-dd HH:mm): ");
            DateTime endTime = ValidationService.IsDateValid();

            if (ValidationService.IsEndDateValid(startTime, endTime))
            {
                codingSession.StartTime = startTime;
                codingSession.EndTime = endTime;

                codingSession.CalculateDuration();

            }
            Controller.UpdateSession(codingSession);
        }

        public void DeleteSession()
        {
            Console.Write("Enter the Id of the session you wish to delete:");
            int.TryParse(Console.ReadLine(), out int deleteId);
            CodingSession codingSession = new CodingSession();
            codingSession.Id = deleteId;
            Controller.DeleteSession(codingSession);
        }

    }
}
