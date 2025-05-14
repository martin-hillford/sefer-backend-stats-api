// This is a general data structure used to represent a histogram. 
// Please note, this class may be used as json output, to don't make the fields private!
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace Sefer.Backend.Stats.Api.DataStructures;

public class Histogram : IHistogram<long, long>
{
    private readonly long _end;

    private readonly long _start;

    public List<Bin<long, long>> Data { get; }

    public long Max { get; private set; } = long.MinValue;

    public long Sum { get; private set; }

    public long BinWidth { get; }

    public Histogram(Dictionary<long, long> data, long binWidth, long start = 0, long? end = null)
    {
        _end = end ?? Math.Max(GetEnd(data), _start);
        _start = start;
        BinWidth = binWidth;

        var bins = (int)Math.Floor((_end - _start) / (double)BinWidth) + 1;
        Data = new List<Bin<long, long>>(bins);
        Initialize(bins);
        Populate(data);
    }

    private void Initialize(long bins)
    {
        for (var index = 0; index < bins; index++)
        {
            Data.Add(new Bin<long, long> { Interval = _start + (BinWidth * index), Quantity = 0 });
        }
    }

    private void Populate(IEnumerable<KeyValuePair<long, long>> data)
    {
        foreach (var (key, value) in data)
        {
            if (key < _start || key > _end) continue;
            var index = (int)Math.Floor((key - _start) / (double)BinWidth);
            Data[index].Quantity += value;
            Sum += value;
            Max = Math.Max(Max, value);
        }
    }

    private static long GetEnd(IEnumerable<KeyValuePair<long, long>> data)
        => data.Select(p => p.Key).DefaultIfEmpty(0).Max();
}