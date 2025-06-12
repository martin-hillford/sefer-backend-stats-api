namespace Sefer.Backend.Stats.Api.Handlers;

public class ApiRequestPerDayHandler(IServiceProvider provider) : CachedRequestHandler<ApiRequestPerDayRequest, List<Bin<int, int>>>(provider)
{
    private const string CacheKey = "ApiRequestLogEntries.LogTime.CountPerDay";

    protected override string GetCacheKey(ApiRequestPerDayRequest request) => CacheKey;

    protected override async Task<List<Bin<int, int>>> GetData(ApiRequestPerDayRequest request, CancellationToken _)
    {
        const string query =
            @"SELECT EXTRACT(DOW FROM log_time) AS interval, COUNT(*) AS quantity
              FROM api_request_log_entries
              GROUP BY EXTRACT(DOW FROM log_time)
              ORDER BY interval";

        var result = await DbConnection.QueryAsync<Bin<int, int>>(query, null, null, 900, null);
        return result.ToList();
    }
}