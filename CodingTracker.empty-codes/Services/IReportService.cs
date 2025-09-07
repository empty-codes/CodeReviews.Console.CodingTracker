using CodingTracker.empty_codes.Models;

namespace CodingTracker.empty_codes.Services
{
    public interface IReportService
    {
        List<CodingSession> FilterSessions(List<CodingSession> sessions, int filterChoice, int sortingChoice);
        void GenerateReport(List<CodingSession> sessions);
    }
}
