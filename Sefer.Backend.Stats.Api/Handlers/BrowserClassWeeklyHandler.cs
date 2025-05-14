namespace Sefer.Backend.Stats.Api.Handlers;

public class BrowserClassWeeklyHandler(IServiceProvider provider) : CachedRequestHandler<BrowserClassWeeklyRequest, List<CategoryItemWeekly>>(provider)
{
    public const string CacheKey = "ClientPageRequestLogEntries.BrowserClass.Weekly";

    protected override string GetCacheKey(BrowserClassWeeklyRequest request) => CacheKey;

    protected override async Task<List<CategoryItemWeekly>> GetData(BrowserClassWeeklyRequest request, CancellationToken cancellationToken)
    {
        const string query =
            @"SELECT browser_class as name, DATE_PART('year',log_time) AS year, DATE_PART('week',log_time) AS week, COUNT(*) AS count
              FROM client_page_request_log_entries
              GROUP BY DATE_PART('year',log_time), DATE_PART('week',log_time), browser_class";

        var result = await DbConnection.QueryAsync<CategoryItemWeekly>(query, null, null, 900, null);
        return result.ToList().CalculatePercentages();
    }
}





