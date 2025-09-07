using CodingTracker.empty_codes.Models;

namespace CodingTracker.empty_codes.Controllers
{
    public interface ICodingController
    {
        void InsertSession(CodingSession session);
        List<CodingSession> ViewAllSessions();
        void UpdateSession(CodingSession session);
        void DeleteSession(CodingSession session);
    }
}
