namespace Sefer.Backend.Stats.Api.Requests;

public class PeriodSummaryStatsRequest(long? lower, long? upper)
    : RangeRequest(lower, upper), IRequest<PeriodSummaryStats> { }