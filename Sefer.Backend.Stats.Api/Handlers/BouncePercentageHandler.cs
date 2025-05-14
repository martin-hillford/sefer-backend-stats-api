namespace Sefer.Backend.Stats.Api.Handlers;

public class BouncePercentageHandler(IServiceProvider provider) : CachedRequestHandler<BouncePercentageRequest, BounceResult>(provider)
{
    protected override string GetCacheKey(BouncePercentageRequest request)
    {
        return request.GetQueryKey();
    }

    protected override async Task<BounceResult> GetData(BouncePercentageRequest request, CancellationToken cancellationToken)
    {
        var paths = request.GetPaths();
        var where = GetWhereClause(paths);
        var parameters = GetParameters(paths, request.UseWildcard);
        var query = GetQuery("bounce-percentage").Replace("$1", where);
        var percentage = await DbConnection.GetScalar<double>(query, parameters);
        return new BounceResult(percentage);
    }

    private static string GetWhereClause(List<string> paths)
    {
        if (paths.Count == 0) return string.Empty;
        var parts = paths.Select((_, index) => $@" path LIKE @path{index} ");
        return " AND " + string.Join("OR", parts);
    }

    private static DynamicParameters GetParameters(IReadOnlyList<string> paths, bool useWildcard)
    {
        var args = new DynamicParameters();
        for (var index = 0; index < paths.Count; index++)
        {
            var path = useWildcard ? paths[index] + "%" : paths[index];
            args.Add("path" + index, path);
        }
        return args;
    }
}