using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CodingTracker.empty_codes.Services
{
    internal static class Validation
    {
       
        public static int IsMenuChoiceValid()
        {
            int choice;
            bool isChoiceValid;

            isChoiceValid = int.TryParse(Console.ReadLine(), out choice);
            while (!isChoiceValid || choice < 1 || choice > 6)
            {
                Console.WriteLine("Error: Unrecognized input, Enter a number from 1 to 6: ");
                isChoiceValid = int.TryParse(Console.ReadLine(), out choice);
            }

            return choice;
        }

        public static DateTime IsDateValid()
        {
            string? dateFormat = ConfigurationManager.AppSettings["DateFormat"];
            DateTime dateChoice;
            bool isDateChoiceValid;

            isDateChoiceValid = DateTime.TryParseExact(Console.ReadLine(), dateFormat, null, System.Globalization.DateTimeStyles.None, out dateChoice);

            while (!isDateChoiceValid)
            {
                Console.Write($"Error: Please use the date format specified: {dateFormat}");
                isDateChoiceValid = DateTime.TryParseExact(Console.ReadLine(), dateFormat, null, System.Globalization.DateTimeStyles.None, out dateChoice);
            }
            return dateChoice;
        }

        public static bool IsEndDateValid(DateTime start, DateTime end)
        {
            if(end <= start)
            {
                Console.WriteLine("Error: End date cannot be before or the same as the start date.");
                return false;
            }
            return true;
        }

    }
}
