namespace Sefer.Backend.Stats.Api.Handlers;

public class ResolutionHandler(IServiceProvider serviceProvider) : CachedRequestHandler<ResolutionRequest, List<Resolution>>(serviceProvider)
{
    public const string CacheKey = "ClientPageRequestLogEntries.Resolutions.Total";

    protected override async Task<List<Resolution>> GetData(ResolutionRequest request, CancellationToken cancellationToken)
    {
        var total = await GetTotalAsync();
        var tiny = await GetResolutionAsync("Tiny", total, null, 479);
        var mobile = await GetResolutionAsync("Mobile", total, 480, 799);
        var small = await GetResolutionAsync("Small", total, 800, 1199);
        var medium = await GetResolutionAsync("Medium", total, 1200, 1511);
        var large = await GetResolutionAsync("Large", total, 1512, null);
        return [tiny, mobile, small, medium, large];
    }

    protected override string GetCacheKey(ResolutionRequest request) => CacheKey;

    private async Task<long> GetTotalAsync()
    {
        const string query = "SELECT COUNT(*) AS count FROM client_page_request_log_entries";
        return await DbConnection.GetScalar<long>(query);
    }

    private async Task<Resolution> GetResolutionAsync(string name, long total, int? min, int? max)
    {
        var whereClause = GetWhereClause(min, max);
        var query = $"SELECT COUNT(*) AS count FROM client_page_request_log_entries WHERE {whereClause}";
        var count = await DbConnection.GetScalar<long>(query);
        return new Resolution
        {
            Count = count,
            Name = name,
            Percentage = (100d * count) / total,
            MinWidth = min,
            MaxWidth = max,
            TotalCount = total
        };
    }

    private static string GetWhereClause(int? min, int? max)
    {
        if (!min.HasValue) return $"screen_width <= {max}";
        if (!max.HasValue) return $"screen_width >= {min}";
        return $"screen_width >= {min} AND screen_width <= {max}";
    }
}
