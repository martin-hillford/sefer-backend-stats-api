namespace Sefer.Backend.Stats.Api.Handlers;

public class ActiveStudentsHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<ActiveStudentsRequest, DateTimeHistogram>
{
    public async Task<DateTimeHistogram> Handle(ActiveStudentsRequest request, CancellationToken _)
    {
        // The problem with counting the active students over a period is that
        // a certain student can be active today and also yesterday and is therefore counted twice.
        var histogram = await GetHistogramAsync(request);
        histogram.Sum = await GetSumAsync(request);
        return histogram;
    }

    private async Task<DateTimeHistogram> GetHistogramAsync(ActiveStudentsRequest request)
    {
        switch (request.BinSize)
        {
            case DateTimeBinSize.Hour:
            case DateTimeBinSize.Second:
                throw new ArgumentException("Active students can't be view in a resolution higher then one day");
            case DateTimeBinSize.Day:
                const string query =
                    $@"SELECT date, count FROM active_students
                        WHERE date BETWEEN @Lower AND @Upper";
                return await DbConnection.QueryHistogram(query, request);
            default:
                const string subQuery =
                    $@"SELECT date::Date FROM active_students
                       WHERE date BETWEEN @Lower AND @Upper";
                return await DbConnection.QueryHistogramUseSubQuery(subQuery, request);
        }
    }

    private async Task<long> GetSumAsync(ActiveStudentsRequest request)
    {
        const string query =
            $@"SELECT COUNT(*) FROM user_last_activity
              JOIN users ON user_last_activity.user_id = users.Id
              WHERE users.role = 1 OR users.role = 2 AND activity_date BETWEEN @Lower AND @Upper";

        var parameters = new { request.Lower, request.Upper };
        return await DbConnection.GetScalar<long>(query, parameters);
    }
}