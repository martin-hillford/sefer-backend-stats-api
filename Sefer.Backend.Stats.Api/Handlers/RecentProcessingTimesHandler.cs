namespace Sefer.Backend.Stats.Api.Handlers;

public class RecentProcessingTimesHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<RecentProcessingTimesRequest, List<DateTimeValue>>
{
    public async Task<List<DateTimeValue>> Handle(RecentProcessingTimesRequest request, CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow.AddHours(-1 * request.Hours).ToString("yyyy-MM-dd HH:mm:ss");
        var query =
            $@"SELECT log_time::Date, api_request_log_entries.processing_time::float8 / 1000 AS value
               FROM api_request_log_entries
               WHERE log_time > '{date}'";

        var result = await DbConnection.QueryAsync<DateTimeValue>(query);
        return result.ToList();
    }
}