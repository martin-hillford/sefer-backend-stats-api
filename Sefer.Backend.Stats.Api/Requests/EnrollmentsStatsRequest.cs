namespace Sefer.Backend.Stats.Api.Requests;

public class EnrollmentsStatsRequest(long? lower, long? upper)
    : RangeRequest(lower, upper), IRequest<EnrollmentStats> { }