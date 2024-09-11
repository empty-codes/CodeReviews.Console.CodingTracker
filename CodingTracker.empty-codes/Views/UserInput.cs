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
                Console.WriteLine("1 to View all Sessions Recorded");
                Console.WriteLine("2 to Insert a Session");
                Console.WriteLine("3 to Update a Session");
                Console.WriteLine("4 to Delete a Session");
                Console.WriteLine("5 to View a Tailored Report");
                Console.WriteLine("6 to Exit this Application");
                Console.WriteLine("________________________\n");
                int choice = Validation.IsMenuChoiceValid();

                switch (choice)
                {
                    case 1:
                        ViewAllSessions();
                        break;
                    case 2:
                        AddSession();
                        break;
                    case 3:
                        UpdateSession();
                        break;
                    case 4:
                        DeleteSession();
                        break;
                    case 5:
                        ViewGeneralReport();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Error: Unrecognized input.");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }


        }

        public void ViewAllSessions()
        {
            var sessions = Controller.ViewAllSessions();

            
        }

        public void AddSession()
        {
            Console.Write("Enter the start time using the 24H format (yyyy-MM-dd HH:mm): ");
            DateTime startTime = Validation.IsDateValid();

            Console.Write("Enter the end time using the 24H format (yyyy-MM-dd HH:mm):4 ");
            DateTime endTime = Validation.IsDateValid();

            if(Validation.IsEndDateValid(startTime, endTime))
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
            DateTime startTime = Validation.IsDateValid();

            Console.Write("Enter the new end time using the 24H format (yyyy-MM-dd HH:mm): ");
            DateTime endTime = Validation.IsDateValid();

            if (Validation.IsEndDateValid(startTime, endTime))
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

        public void ViewGeneralReport()
        {

        }
    }
}
