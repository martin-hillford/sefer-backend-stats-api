namespace Sefer.Backend.Stats.Api.Handlers;

public class DashboardStatsHandler(IDbConnectionProvider provider) : DbConnectionProviderHandler(provider), IRequestHandler<DashboardStatsRequest, DashboardStats>
{
    public async Task<DashboardStats> Handle(DashboardStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new DashboardStats();
        var tasks = new List<Task>
        {
            LoadActiveStudentsToday(request, stats),
            LoadActiveMentorsToday(request, stats),
            LoadMessagesSendToday(request, stats),
            LoadTotalLessonsSubmitted(stats),
            LoadTotalCompletedCourses(stats),
            LoadTotalStudents(stats),
            LoadCurrentActiveStudents(stats)
        };

        await Task.WhenAll(tasks);
        return stats;
    }

    private async Task LoadActiveStudentsToday(DashboardStatsRequest request, DashboardStats stats)
    {
        const string sql = @"SELECT count FROM active_students WHERE date::Date = @Today";
        stats.ActiveStudentsToday = await GetConnection().GetScalar<long>(sql, request);
    }

    private async Task LoadActiveMentorsToday(DashboardStatsRequest request, DashboardStats stats)
    {
        const string sql = @"SELECT count FROM active_mentors WHERE date::Date = @Today";
        stats.ActiveMentorsToday = await GetConnection().GetScalar<long>(sql, request);
    }

    private async Task LoadMessagesSendToday(DashboardStatsRequest request, DashboardStats stats)
    {
        var input = new RangeRequest(request.Today, request.Today);
        stats.MessagesSendToday = await SharedQueries.GetMessagesSend(GetConnection(), input);
    }

    private async Task LoadTotalStudents(DashboardStats stats)
    {
        const string sql = @"SELECT COUNT(id) FROM users WHERE role = 1 OR role = 2";
        stats.TotalStudents = await GetConnection().GetScalar<long>(sql);
    }

    private async Task LoadTotalCompletedCourses(DashboardStats stats)
    {
        const string sql = @"SELECT COUNT(Id) FROM enrollments WHERE is_course_completed = TRUE";
        stats.TotalCompletedCourses = await GetConnection().GetScalar<long>(sql);
    }

    private async Task LoadTotalLessonsSubmitted(DashboardStats stats)
    {
        const string sql =
            @"SELECT COUNT(id) FROM lesson_submissions WHERE submission_date IS NOT NUll AND is_final = TRUE";
        stats.TotalLessonsSubmitted = await GetConnection().GetScalar<long>(sql);
    }

    private async Task LoadCurrentActiveStudents(DashboardStats stats)
    {
        const string sql =
            @"SELECT COUNT(*) FROM user_last_activity as l
              JOIN users AS u ON u.Id = l.user_id
              WHERE l.activity_date >= (SELECT  NOW() - (MAX(student_active_days) * INTERVAL '1 day')  FROM settings) AND role < 3";
        stats.CurrentActiveStudents = await GetConnection().GetScalar<long>(sql);
    }
}