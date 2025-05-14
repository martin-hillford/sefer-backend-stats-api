namespace Sefer.Backend.Stats.Api.Util;

public static class DbConnectionExtensions
{
    public static Task<DateTimeHistogram> QueryHistogramUseSubQuery<T>(this IDbConnection connection, string subQuery, T request)
        where T : BinnedRangeRequest
    {
        var sql = $@"SELECT e.date AS date, COUNT(date) AS count FROM ( {subQuery} ) AS e GROUP BY e.date";
        return connection.QueryHistogram(sql, request);
    }

    public static async Task<DateTimeHistogram> QueryHistogram<T>(this IDbConnection connection, string sql, T request)
        where T : BinnedRangeRequest
    {
        var data = await connection.QueryAsync<DateTimeCount>(sql, request.GetParameters());

        var histogram = new DateTimeHistogram(request);
        histogram.Add(data, e => e.Date, (value, current) => value.Count + current);

        return histogram;
    }

    public static Task<T?> GetScalar<T>(this IDbConnection connection, string sql, object? parameters = null)
        => connection.ExecuteScalarAsync<T>(sql, parameters);
}
