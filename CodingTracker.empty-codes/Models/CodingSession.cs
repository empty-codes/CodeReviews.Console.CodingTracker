namespace CodingTracker.empty_codes.Models;

internal class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public void CalculateDuration()
    {
        if (EndTime > StartTime)
        {
            Duration = EndTime - StartTime;
        }
        else
        {
            throw new Exception("EndTime must be after StartTime.");
        }
    }
}