namespace Sefer.Backend.Stats.Api.Requests;

public class RecentProcessingTimesRequest(ushort hours) : IRequest<List<DateTimeValue>>
{
    public readonly ushort Hours = hours;
}