namespace Sefer.Backend.Stats.Api.Requests;

public class CompletedEnrollmentsRequest(long? lower, long? upper, int? binSize)
    : BinnedRangeRequest(lower, upper, binSize), IRequest<DateTimeHistogram> {  }