namespace Sefer.Backend.Stats.Api.Handlers;

public class ActiveMentorsHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<ActiveMentorsRequest, DateTimeHistogram>
{
    public async Task<DateTimeHistogram> Handle(ActiveMentorsRequest request, CancellationToken cancellationToken)
    {
        switch (request.BinSize)
        {
            case DateTimeBinSize.Hour:
            case DateTimeBinSize.Second:
                throw new ArgumentException("Active mentors can't be view in a resolution higher then one day");
            case DateTimeBinSize.Day:
                const string query =
                    $@"SELECT date, count FROM active_mentors
                        WHERE date BETWEEN @Lower AND @Upper";
                return await DbConnection.QueryHistogram(query, request);
            default:
                const string subQuery =
                    $@"SELECT date::Date FROM active_mentors
                       WHERE date BETWEEN @Lower AND @Upper";
                return await DbConnection.QueryHistogramUseSubQuery(subQuery, request);
        }
    }
}