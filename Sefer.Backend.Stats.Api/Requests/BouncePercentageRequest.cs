
namespace Sefer.Backend.Stats.Api.Requests;

public class BouncePercentageRequest : IRequest<BounceResult>
{
    private List<string> Paths { get; set; } = [];

    public bool UseWildcard { get; set; } = false;

    public List<string> GetPaths()
    {
        return Paths.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
    }

    public string GetQueryKey()
    {
        var paths = string.Join('_', GetPaths());
        return paths + (UseWildcard ? "*" : string.Empty);
    }
}