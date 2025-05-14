namespace Sefer.Backend.Stats.Api.Handlers;

public class VisitsHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<VisitsRequest, DateTimeHistogram>
{
    public async Task<DateTimeHistogram> Handle(VisitsRequest request, CancellationToken cancellationToken)
    {
        const string sql =
            $@"SELECT log_time::Date AS date FROM client_page_request_log_entries
               WHERE path LIKE @Path AND is_unique_visit = TRUE";

        return await DbConnection.QueryHistogramUseSubQuery(sql, request);
    }
}