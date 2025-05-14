namespace Sefer.Backend.Stats.Api.Handlers;

public class NewEnrollmentsHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<NewEnrollmentsRequest, DateTimeHistogram>
{
    public async Task<DateTimeHistogram> Handle(NewEnrollmentsRequest request, CancellationToken cancellationToken)
    {
        const string sql =
            $@"SELECT creation_date::Date AS date FROM enrollments
               WHERE creation_date BETWEEN @Lower AND @Upper";

        return await DbConnection.QueryHistogramUseSubQuery(sql, request);
    }
}