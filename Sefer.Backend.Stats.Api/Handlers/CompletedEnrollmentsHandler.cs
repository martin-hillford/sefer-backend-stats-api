namespace Sefer.Backend.Stats.Api.Handlers;

public class CompletedEnrollmentsHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<CompletedEnrollmentsRequest, DateTimeHistogram>
{
    public async Task<DateTimeHistogram> Handle(CompletedEnrollmentsRequest request, CancellationToken cancellationToken)
    {
        const string sql =
            $@"SELECT closure_date::Date AS date FROM enrollments
               WHERE closure_date BETWEEN @Lower AND @Upper AND closure_date IS NOT NULL";

        return await DbConnection.QueryHistogramUseSubQuery(sql, request);
    }
}