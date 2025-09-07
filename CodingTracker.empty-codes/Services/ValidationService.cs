namespace CodingTracker.empty_codes.Services;

public class ValidationService : IValidationService
{
    private readonly IConsoleService Console;

    public ValidationService(IConsoleService console)
    {
        Console = console;
    }
    public int IsMenuChoiceValid(int min, int max)
    {
        int choice;
        bool isChoiceValid;

        isChoiceValid = int.TryParse(Console.ReadLine(), out choice);
        while (!isChoiceValid || choice < min || choice > max)
        {
            Console.WriteLine($"[red]Error: Unrecognized input. Enter a number from {min} to {max}: [/]");
            isChoiceValid = int.TryParse(Console.ReadLine(), out choice);
        }
        return choice;
    }

    public DateTime IsDateValid(string? input)
    {
        string? dateFormat = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
        DateTime dateChoice;
        bool isDateChoiceValid;

        isDateChoiceValid = DateTime.TryParseExact(input, dateFormat, null, System.Globalization.DateTimeStyles.None, out dateChoice);

        while (!isDateChoiceValid)
        {
            Console.WriteLine($"[red]Error: Please use the correct date format: {dateFormat}[/]");
            isDateChoiceValid = DateTime.TryParseExact(Console.ReadLine(), dateFormat, null, System.Globalization.DateTimeStyles.None, out dateChoice);
        }
        return dateChoice;
    }

    public bool IsEndDateValid(DateTime start, DateTime end)
    {
        if (end <= start)
        {
            Console.WriteLine("[red]Error: End date cannot be before or the same as the start date.[/]");
            return false;
        }
        return true;
    }
}