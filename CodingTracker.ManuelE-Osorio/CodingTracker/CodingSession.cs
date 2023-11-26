using System.Diagnostics;
using System.Globalization;

class CodingSession
{
    public DateTime StartDateTime;
    public DateTime EndDateTime;

    public TimeSpan ElapsedTime;

    public int ID;

    public CodingSession(string? id, string? startDate, string? startTime, string? endDate, string? endTime)
    {
        DateTime.TryParseExact(startDate+" "+startTime, "yyyy/MM/dd HH:mm",CultureInfo.InvariantCulture,
        DateTimeStyles.None,out StartDateTime);
        DateTime.TryParseExact(endDate+" "+endTime, "yyyy/MM/dd HH:mm",CultureInfo.InvariantCulture,
        DateTimeStyles.None,out DateTime TempDateTime);
        EndDateTime = TempDateTime;
        ID = Convert.ToInt32(id);
        ElapsedTime = EndDateTime - StartDateTime;
    }

    public CodingSession(int id, DateTime startDateTime, DateTime endDateTime, TimeSpan elapsedTime)
    {
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        ID = id;
        ElapsedTime = elapsedTime;
    }

    public string[] GetString()
    {
        string[] codingSessionString = new string[6];
        
        codingSessionString[0] = StartDateTime.Date.ToString("yyyy/MM/dd");
        codingSessionString[1] = StartDateTime.TimeOfDay.ToString("hh\\:mm");
        codingSessionString[2] = EndDateTime.Date.ToString("yyyy/MM/dd");
        codingSessionString[3] = EndDateTime.TimeOfDay.ToString("hh\\:mm");
        codingSessionString[4] = ElapsedTime.ToString("d\\.hh\\:mm");
        codingSessionString[5] = ID.ToString();
        return codingSessionString;
    }
}