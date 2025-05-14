namespace Sefer.Backend.Stats.Api.Handlers;

public class SubmittedLessonsHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<SubmittedLessonsRequest, DateTimeHistogram>
{
    public async Task<DateTimeHistogram> Handle(SubmittedLessonsRequest request, CancellationToken cancellationToken)
    {
        const string sql =
            $@"SELECT submission_date::Date AS date FROM lesson_submissions
               WHERE submission_date BETWEEN @Lower AND @Upper AND is_final = TRUE";

        return await DbConnection.QueryHistogramUseSubQuery(sql, request);
    }
}