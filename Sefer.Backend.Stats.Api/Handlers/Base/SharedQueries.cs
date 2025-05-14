namespace Sefer.Backend.Stats.Api.Handlers.Base;

public static class SharedQueries
{
    public static async Task<long> GetEnrollmentsCount(IDbConnection connection, RangeRequest request)
    {
        const string sql = @"SELECT COUNT(id) FROM enrollments WHERE creation_date BETWEEN @Lower AND @Upper";
        return await connection.GetScalar<long>(sql, request.GetParameters());
    }

    public static async Task<long> GetCompletedEnrollmentsCount(IDbConnection connection, RangeRequest request)
    {
        const string sql =
            @"SELECT COUNT(id) FROM enrollments
               WHERE creation_date BETWEEN @Lower AND @Upper AND
                     closure_date IS NOT NULL AND is_course_completed = TRUE";
        return await connection.GetScalar<long>(sql, request.GetParameters());
    }

    public static async Task<long> GetClosedEnrollmentsCount(IDbConnection connection, RangeRequest request)
    {
        const string sql =
            $@"SELECT COUNT(id) FROM enrollments
               WHERE closure_date IS NOT NULL AND is_course_completed = FALSE AND
                      creation_date BETWEEN @Lower AND @Upper";
        return await connection.GetScalar<long>(sql, request.GetParameters());
    }

    public static async Task<long> GetMessagesSend(IDbConnection connection, RangeRequest request)
    {
        const string sql = "SELECT COUNT(id) FROM chat_messages WHERE sender_date BETWEEN @Lower AND @Upper";
        return await connection.GetScalar<long>(sql, request.GetParameters());
    }
}