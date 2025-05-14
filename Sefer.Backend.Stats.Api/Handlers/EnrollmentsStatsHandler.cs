namespace Sefer.Backend.Stats.Api.Handlers;

public class EnrollmentsStatsHandler(IDbConnectionProvider provider) : DbConnectionProviderHandler(provider), IRequestHandler<EnrollmentsStatsRequest, EnrollmentStats>
{
    public async Task<EnrollmentStats> Handle(EnrollmentsStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new EnrollmentStats();

        var tasks = new List<Task>
        {
            LoadEnrollmentsCount(request, stats),
            LoadCompletedEnrollmentsCount(request, stats),
            LoadClosedEnrollmentsCount(request, stats),
            LoadEnrollmentsByMenCount(request,stats),
            LoadEnrollmentsByWomenCount(request, stats),
        };

        await Task.WhenAll(tasks);
        return stats;
    }

    private async Task LoadEnrollmentsCount(RangeRequest request, EnrollmentStats stats)
    {
        stats.Total = await SharedQueries.GetEnrollmentsCount(GetConnection(), request);
    }

    private async Task LoadCompletedEnrollmentsCount(RangeRequest request, EnrollmentStats stats)
    {
        stats.Completed = await SharedQueries.GetCompletedEnrollmentsCount(GetConnection(), request);
    }

    private async Task LoadClosedEnrollmentsCount(RangeRequest request, EnrollmentStats stats)
    {
        stats.Closed = await SharedQueries.GetClosedEnrollmentsCount(GetConnection(), request);
    }

    private async Task LoadEnrollmentsByMenCount(RangeRequest request, EnrollmentStats stats)
    {
        stats.Males = await LoadEnrollmentsByGenderCount(request, 1);
    }

    private async Task LoadEnrollmentsByWomenCount(RangeRequest request, EnrollmentStats stats)
    {
        stats.Females = await LoadEnrollmentsByGenderCount(request, 2);
    }

    private async Task<long> LoadEnrollmentsByGenderCount(RangeRequest request, int gender)
    {
        const string sql =
            $@"SELECT COUNT(e.id) FROM enrollments AS e
               JOIN users AS u ON e.student_id = u.id
               WHERE creation_date BETWEEN @Lower AND @Upper AND
                     closure_date IS NOT NULL AND is_course_completed = FALSE AND
                     gender = @Gender";
        var parameters = new { request.Lower, request.Upper, Gender = gender };
        return await GetConnection().GetScalar<long>(sql, parameters);
    }
}
