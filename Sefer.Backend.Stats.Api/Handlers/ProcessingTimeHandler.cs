namespace Sefer.Backend.Stats.Api.Handlers;

public class ProcessingTimeHandler(IServiceProvider provider) : CachedRequestHandler<ProcessingTimeRequest, List<ProcessingTimeResult>>(provider)
{
    public const string CacheKey = "ApiRequestLogEntries.ProcessingTime.Histogram";

    protected override string GetCacheKey(ProcessingTimeRequest request) => CacheKey;

    protected override async Task<List<ProcessingTimeResult>> GetData(ProcessingTimeRequest request, CancellationToken cancellationToken)
    {
        var query = GetQuery("processing-time");
        var result = await DbConnection.QueryAsync<ProcessingTimeResult>(query, null, null, 900, null);
        return result.ToList();
    }
}