namespace Sefer.Backend.Stats.Api.Handlers;

public class NewStudentsHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<NewStudentsRequest, DateTimeHistogram>
{
    public async Task<DateTimeHistogram> Handle(NewStudentsRequest request, CancellationToken cancellationToken)
    {
        const string sql =
            $@"SELECT subscription_date::Date AS date FROM users
               WHERE subscription_date BETWEEN @Lower AND @Upper AND  (role = 1 OR role = 2)";

        return await DbConnection.QueryHistogramUseSubQuery(sql, request);
    }
}