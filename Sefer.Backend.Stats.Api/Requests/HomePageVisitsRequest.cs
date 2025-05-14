namespace Sefer.Backend.Stats.Api.Requests;

public class HomePageVisitsRequest(long? lower, long? upper, int? binSize)
    : BinnedRangeRequest(lower, upper, binSize), IRequest<DateTimeHistogram> { }