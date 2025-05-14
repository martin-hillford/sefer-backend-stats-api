namespace Sefer.Backend.Stats.Api.Requests.Base;

public class RangeRequest
{
    public readonly DateTime Lower;

    public readonly DateTime Upper;

    public RangeRequest(DateTime lower, DateTime upper)
    {
        Lower = lower;
        Upper = upper;
    }
    
    protected RangeRequest(long? lower, long? upper)
    {
        Upper = FromUnixTime(upper) ?? DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
        Lower = FromUnixTime(lower) ?? Upper.AddMonths(-3).Date;
    }
    
    private static DateTime? FromUnixTime(long? timestamp)
    {
        if (timestamp == null) return null;
        return (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(timestamp.Value);
    }
    
    public virtual object GetParameters()
    {
        return new
        {
            Lower = Lower.Date.AddDays(-1).AddSeconds(1),
            Upper = Upper.Date.AddDays(1).AddSeconds(-1),
        };
    }
}