using CodingTracker.empty_codes.Models;

namespace CodingTracker.empty_codes.Services
{
    public interface IValidationService
    {
        int IsMenuChoiceValid(int min, int max);
        DateTime IsDateValid(string? input);
        bool IsEndDateValid(DateTime start, DateTime end);


    }
}
