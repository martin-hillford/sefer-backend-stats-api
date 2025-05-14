namespace Sefer.Backend.Stats.Api.Requests;

public class VisitsRequest(long? lower, long? upper, int? binSize, string? path) : BinnedRangeRequest(lower, upper, binSize), IRequest<DateTimeHistogram>
{
    public VisitsRequest(VisitsModel model)
        : this(model.Lower, model.Upper, (int)model.BinSize, model.Path) { }

    public override object GetParameters()
    {
        return new
        {
            Lower = Lower.Date.AddDays(-1).AddSeconds(1),
            Upper = Upper.Date.AddDays(1).AddSeconds(-1),
            Path = path
        };
    }
}