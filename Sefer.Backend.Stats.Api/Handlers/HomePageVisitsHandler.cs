namespace Sefer.Backend.Stats.Api.Handlers;

public class HomePageVisitsHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<HomePageVisitsRequest, DateTimeHistogram>
{
    public async Task<DateTimeHistogram> Handle(HomePageVisitsRequest request, CancellationToken cancellationToken)
    {
        const string sql =
        $@"SELECT log_time::Date AS date FROM client_page_request_log_entries
               WHERE (path = '/' OR path = '/home' OR path = '/') AND is_unique_visit = TRUE";

        return await DbConnection.QueryHistogramUseSubQuery(sql, request);
    }
}

