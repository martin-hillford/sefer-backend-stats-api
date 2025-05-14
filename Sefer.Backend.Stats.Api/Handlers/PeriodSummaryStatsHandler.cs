namespace Sefer.Backend.Stats.Api.Handlers;

public class PeriodSummaryStatsHandler(IDbConnectionProvider provider) : DbConnectionProviderHandler(provider), IRequestHandler<PeriodSummaryStatsRequest, PeriodSummaryStats>
{
    public async Task<PeriodSummaryStats> Handle(PeriodSummaryStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new PeriodSummaryStats();

        var tasks = new List<Task>
        {
            LoadEnrollmentsCount(request, stats),
            LoadCompletedEnrollmentsCount(request, stats),
            LoadClosedEnrollmentsCount(request, stats),
            LoadMessagesSend(request, stats),
            LoadActiveStudents(request, stats),
            LoadNewStudents(request, stats),
            LoadLessonsSubmitted(request, stats),
            LoadCompletedCourses(request, stats),
            LoadAverageReviewTime(request, stats)
        };

        await Task.WhenAll(tasks);
        return stats;
    }

    private async Task LoadEnrollmentsCount(RangeRequest request, PeriodSummaryStats stats)
    {
        stats.NewEnrollments = await SharedQueries.GetEnrollmentsCount(GetConnection(), request);
    }

    private async Task LoadCompletedEnrollmentsCount(RangeRequest request, PeriodSummaryStats stats)
    {
        stats.CompletedCourses = await SharedQueries.GetCompletedEnrollmentsCount(GetConnection(), request);
    }

    private async Task LoadClosedEnrollmentsCount(RangeRequest request, PeriodSummaryStats stats)
    {
        stats.ClosedCourses = await SharedQueries.GetClosedEnrollmentsCount(GetConnection(), request);
    }

    private async Task LoadMessagesSend(RangeRequest request, PeriodSummaryStats stats)
    {
        stats.Messages = await SharedQueries.GetMessagesSend(GetConnection(), request);
    }

    private async Task LoadActiveStudents(RangeRequest request, PeriodSummaryStats stats)
    {
        const string sql =
            $@"SELECT COUNT(DISTINCT user_id) FROM login_log_entries
               WHERE user_id IS NOT NULL AND log_time BETWEEN @Lower AND @Upper";
        stats.ActiveStudents = await GetConnection().GetScalar<long>(sql, request.GetParameters());
    }

    private async Task LoadNewStudents(RangeRequest request, PeriodSummaryStats stats)
    {
        const string sql =
            $@"SELECT COUNT(id) FROM users
               WHERE subscription_date BETWEEN @Lower AND @Upper AND  (Role = 1 OR Role = 2)";
        stats.NewStudents = await GetConnection().GetScalar<long>(sql, request.GetParameters());
    }

    private async Task LoadLessonsSubmitted(RangeRequest request, PeriodSummaryStats stats)
    {
        const string sql =
            @"SELECT COUNT(id) FROM lesson_submissions
              WHERE submission_date BETWEEN @Lower AND @Upper AND is_final = TRUE";
        stats.SubmittedLessons = await GetConnection().GetScalar<long>(sql, request.GetParameters());
    }

    private async Task LoadCompletedCourses(RangeRequest request, PeriodSummaryStats stats)
    {
        const string sql =
            @"SELECT COUNT(id) FROM enrollments
              WHERE is_course_completed = TRUE AND closure_date BETWEEN @Lower AND @Upper";
        stats.CompletedCourses = await GetConnection().GetScalar<long>(sql, request.GetParameters());
    }

    private async Task LoadAverageReviewTime(RangeRequest request, PeriodSummaryStats stats)
    {
        const string sql =
            @"SELECT avg(review_time) FROM lesson_review_time WHERE submission_date BETWEEN @Lower AND @Upper";
        stats.AverageReviewTime = await GetConnection().GetScalar<double>(sql, request.GetParameters());
    }
}