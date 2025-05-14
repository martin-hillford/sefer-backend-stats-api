namespace Sefer.Backend.Stats.Api.Handlers;

public class OperatingSystemHandler(IServiceProvider provider) : CachedRequestHandler<OperatingSystemRequest, List<CategoryItem>>(provider)
{
    public const string CacheKey = "ClientPageRequestLogEntries.OperatingSystem";

    protected override string GetCacheKey(OperatingSystemRequest request) => CacheKey;

    protected override async Task<List<CategoryItem>> GetData(OperatingSystemRequest request, CancellationToken cancellationToken)
    {
        const string query =
            @"SELECT operating_system AS name, COUNT(*) AS count, COUNT(*) * 100.0 / SUM(COUNT(*)) OVER() AS percentage
            FROM client_page_request_log_entries GROUP BY operating_system";

        var result = await DbConnection.QueryAsync<CategoryItem>(query, null, null, 900, null);
        return result.ToList();
    }
}