namespace Sefer.Backend.Stats.Api.Handlers;

public class ApiRequestPerHourHandler(IServiceProvider provider) : CachedRequestHandler<ApiRequestPerHourRequest, List<Bin<int, int>>>(provider)
{
    public const string CacheKey = "ApiRequestLogEntries.LogTime.CountPerHourOfTheWeek";

    protected override string GetCacheKey(ApiRequestPerHourRequest request) => CacheKey;

    protected override async Task<List<Bin<int, int>>> GetData(ApiRequestPerHourRequest request, CancellationToken _)
    {
        const string query =
            @"SELECT DATE_PART('hour', log_time) AS interval, COUNT(*) AS quantity
              FROM api_request_log_entries
              GROUP BY DATE_PART('hour', log_time)
              ORDER BY interval";

        var result = await DbConnection.QueryAsync<Bin<int, int>>(query, null, null, 900, null);
        return result.ToList();
    }
}