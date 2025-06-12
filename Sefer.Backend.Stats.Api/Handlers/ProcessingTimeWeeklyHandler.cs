namespace Sefer.Backend.Stats.Api.Handlers;

public class ProcessingTimeWeeklyHandler(IServiceProvider provider) : CachedRequestHandler<ProcessingTimeWeeklyRequest, List<ProcessingTimeWeekly>>(provider)
{
    public const string CacheKey = "ApiRequestLogEntries.ProcessingTime.Weekly";

    protected override string GetCacheKey(ProcessingTimeWeeklyRequest request) => CacheKey;

    protected override async Task<List<ProcessingTimeWeekly>> GetData(ProcessingTimeWeeklyRequest request, CancellationToken cancellationToken)
    {
        const string query =
            @"SELECT AVG(processing_time) as average, DATE_PART('year',log_time) AS year, DATE_PART('week',log_time) AS week, COUNT(*) AS count
              FROM api_request_log_entries
              GROUP BY DATE_PART('year',log_time), DATE_PART('week',log_time)
             ORDER BY year, week";
        var result = await DbConnection.QueryAsync<ProcessingTimeWeekly>(query, null, null, 900, null);
        return result.ToList();
    }
}