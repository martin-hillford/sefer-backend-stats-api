namespace Sefer.Backend.Stats.Api.Handlers;

public class StudentAgeHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<StudentAgeRequest, Histogram>
{
    public async Task<Histogram> Handle(StudentAgeRequest request, CancellationToken cancellationToken)
    {
        var sql = GetQuery(request);

        var result = await DbConnection.QueryAsync(sql, request);
        var data = result.ToDictionary(x => (long)x.Age, x => (long)x.Count);
        return new Histogram(data, 5);
    }

    private static string GetQuery(StudentAgeRequest request)
        => request.CourseId.HasValue ? GetQueryForCourse() : GetQuery();

    private static string GetQuery()
    {
        return
            $@"SELECT (DATE_PART('year', NOW()) - year_of_birth) AS age, COUNT(year_of_birth) AS count
               FROM users WHERE role = 1 OR role = 2 GROUP BY year_of_birth";
    }

    private static string GetQueryForCourse()
    {
        return
            $@"SELECT (DATE_PART('year', NOW()) - year_of_birth) AS age, COUNT(year_of_birth) AS Count
               FROM users
               JOIN enrollments ON users.Id = enrollments.student_id
               JOIN course_revisions ON enrollments.course_revision_id = course_revisions.id
               WHERE (role = 1 OR role = 2) AND course_id = :CourseId GROUP BY year_of_birth";
    }
}