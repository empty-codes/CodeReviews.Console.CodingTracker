namespace CodingTracker.empty_codes.Services;

public class ValidationService : IValidationService
{
    private readonly IConsoleService _console;
    private readonly string _dateFormat;

    public ValidationService(IConsoleService console, string? dateFormat = null)
    {
        _console = console;
        _dateFormat = dateFormat;
    }
    public int IsMenuChoiceValid(int min, int max)
    {
        int choice;
        bool isChoiceValid;

        isChoiceValid = int.TryParse(_console.ReadLine(), out choice);
        while (!isChoiceValid || choice < min || choice > max)
        {
            _console.WriteLine($"[red]Error: Unrecognized input. Enter a number from {min} to {max}: [/]");
            isChoiceValid = int.TryParse(_console.ReadLine(), out choice);
        }
        return choice;
    }

    public DateTime IsDateValid(string? input)
    {
        DateTime dateChoice;
        bool isDateChoiceValid;

        isDateChoiceValid = DateTime.TryParseExact(input, _dateFormat, null, System.Globalization.DateTimeStyles.None, out dateChoice);

        while (!isDateChoiceValid)
        {
            _console.WriteLine($"[red]Error: Please use the correct date format: {_dateFormat}[/]");
            isDateChoiceValid = DateTime.TryParseExact(_console.ReadLine(), _dateFormat, null, System.Globalization.DateTimeStyles.None, out dateChoice);
        }
        return dateChoice;
    }

    public bool IsEndDateValid(DateTime start, DateTime end)
    {
        if (end <= start)
        {
            _console.WriteLine("[red]Error: End date cannot be before or the same as the start date.[/]");
            return false;
        }
        return true;
    }
}