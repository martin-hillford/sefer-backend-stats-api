namespace Sefer.Backend.Stats.Api.Handlers;

public class OperatingSystemWeeklyHandler(IServiceProvider provider) : CachedRequestHandler<OperatingSystemWeeklyRequest, List<CategoryItemWeekly>>(provider)
{
    public const string CacheKey = "ClientPageRequestLogEntries.OperatingSystem.Weekly";

    protected override string GetCacheKey(OperatingSystemWeeklyRequest request) => CacheKey;

    protected override async Task<List<CategoryItemWeekly>> GetData(OperatingSystemWeeklyRequest request, CancellationToken cancellationToken)
    {
        const string query =
            @"SELECT operating_system AS name, DATE_PART('year',log_time) AS year, DATE_PART('week',log_time) AS week, COUNT(*) AS count
              FROM client_page_request_log_entries
              GROUP BY DATE_PART('year',log_time), DATE_PART('week',log_time), operating_system";

        var result = await DbConnection.QueryAsync<CategoryItemWeekly>(query, null, null, 900, null);
        return result.ToList().CalculatePercentages();
    }
}