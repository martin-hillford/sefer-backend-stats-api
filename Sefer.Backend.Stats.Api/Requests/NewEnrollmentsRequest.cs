namespace Sefer.Backend.Stats.Api.Requests;

public class NewEnrollmentsRequest(long? lower, long? upper, int? binSize)
    : BinnedRangeRequest(lower, upper, binSize), IRequest<DateTimeHistogram> { }