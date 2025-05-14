namespace Sefer.Backend.Stats.Api.Handlers;

public class BrowserClassHandler(IServiceProvider provider) : CachedRequestHandler<BrowserClassRequest, List<CategoryItem>>(provider)
{
    public const string CacheKey = "ClientPageRequestLogEntries.BrowserClass";

    protected override string GetCacheKey(BrowserClassRequest request) => CacheKey;

    protected override async Task<List<CategoryItem>> GetData(BrowserClassRequest request, CancellationToken cancellationToken)
    {
        const string query =
            @"SELECT browser_class as name, COUNT(*) AS count, COUNT(*) * 100.0 / SUM(COUNT(*)) OVER() AS percentage
            FROM client_page_request_log_entries GROUP BY browser_class";

        var result = await DbConnection.QueryAsync<CategoryItem>(query, null, null, 900, null);
        return result.ToList();
    }
}



