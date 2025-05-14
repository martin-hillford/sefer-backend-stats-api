namespace Sefer.Backend.Stats.Api.Requests.Base;

public abstract class BinnedRangeRequest(long? lower, long? upper, int? binSize) : RangeRequest(lower, upper)
{
    public readonly DateTimeBinSize BinSize = binSize.HasValue ? (DateTimeBinSize)binSize : DateTimeBinSize.Day;

    public override object GetParameters()
    {
        return new
        {
            Lower = Lower.Date.AddDays(-1).AddSeconds(1),
            Upper = Upper.Date.AddDays(1).AddSeconds(-1)
        };
    }
}