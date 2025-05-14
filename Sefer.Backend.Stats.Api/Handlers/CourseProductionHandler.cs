namespace Sefer.Backend.Stats.Api.Handlers;

public class CourseProductionHandler(IDbConnectionProvider provider) : DatabaseRequestHandler(provider), IRequestHandler<CourseProductionRequest, List<CourseProduction>>
{
    public async Task<List<CourseProduction>> Handle(CourseProductionRequest request, CancellationToken cancellationToken)
    {
        var query = GetQuery("course_production").Replace("$1", request.ActiveDays.ToString());
        var result = await DbConnection.QueryAsync<CourseProduction>(query);
        return result.ToList();
    }
}