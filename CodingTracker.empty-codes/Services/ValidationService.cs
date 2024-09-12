using System.Configuration;
using Spectre.Console;

namespace CodingTracker.empty_codes.Services;

internal static class ValidationService
{
    public static int IsMenuChoiceValid(int min, int max)
    {
        int choice;
        bool isChoiceValid;

        isChoiceValid = int.TryParse(Console.ReadLine(), out choice);
        while (!isChoiceValid || choice < min || choice > max)
        {
            AnsiConsole.MarkupLine($"[red]Error: Unrecognized input. Enter a number from {min} to {max}: [/]");
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
            AnsiConsole.MarkupLine($"[red]Error: Please use the correct date format: {dateFormat}[/]");
            isDateChoiceValid = DateTime.TryParseExact(Console.ReadLine(), dateFormat, null, System.Globalization.DateTimeStyles.None, out dateChoice);
        }
        return dateChoice;
    }

    public static bool IsEndDateValid(DateTime start, DateTime end)
    {
        if (end <= start)
        {
            AnsiConsole.MarkupLine("[red]Error: End date cannot be before or the same as the start date.[/]");
            return false;
        }
        return true;
    }
}