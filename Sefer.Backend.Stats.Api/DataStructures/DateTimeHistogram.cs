// This is a general data structure used to represent a datetime histogram. 
// Please note, this class may be used as json output, to don't make the fields private!
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace Sefer.Backend.Stats.Api.DataStructures;

public class DateTimeHistogram(BinnedRangeRequest request) : DateTimeHistogram<double>(request), IHistogram<DateTime, double>
{
    public new List<Bin<DateTime, double>> Data
        => base.Data.Select(d => new Bin<DateTime, double> { Interval = d.Key, Quantity = d.Value }).ToList();

    public double Max => base.Data.Values.DefaultIfEmpty(0).Max();

    private double? _sum;

    public double Sum
    {
        // Please note: in some situations counting the daily sum
        // will lead to over counting
        // (e.q. a student active today and active tomorrow will be counted double)
        get => _sum ??= base.Data.Values.DefaultIfEmpty(0).Sum();
        set => _sum = value;
    }

    public double Percentile(int k) => base.Data.Values.ToList().Percentile(k);

    public double Median => Percentile(50);
}

public abstract class DateTimeHistogram<T>
{
    public readonly DateTimeBinSize BinSize;

    protected readonly Dictionary<DateTime, T?> Data = [];

    public readonly DateTime LowerBound;

    public readonly DateTime UpperBound;

    protected DateTimeHistogram(BinnedRangeRequest request)
        : this(request.Lower, request.Upper, request.BinSize) { }

    private DateTimeHistogram(DateTime lower, DateTime upper, DateTimeBinSize binSize)
    {
        BinSize = binSize;

        LowerBound = GetBinningDateTime(lower);
        UpperBound = GetBinningDateTime(upper);

        InitializeDictionary();
    }

    public void AddDataPoint(DateTime time, T value)
    {
        var binNumber = GetBinningDateTime(time);
        if (Data.ContainsKey(binNumber) == false) return;
        Data[binNumber] = value;
    }

    public void Add<TObjectType>(IEnumerable<TObjectType> values, Func<TObjectType, DateTime> getTime, Func<TObjectType, T?, T> getValue)
    {
        foreach (var value in values)
        {
            var binNumber = GetBinningDateTime(getTime(value));
            if (Data.ContainsKey(binNumber) == false) continue;
            Data[binNumber] = getValue(value, Data[binNumber]);
        }
    }

    public Dictionary<DateTime, double> Map(Func<T, double> map)
    {
        var result = new Dictionary<DateTime, double>();
        foreach (var (key, value) in Data)
        {
            if (value != null) result.Add(key, map(value));
        }
        return result;
    }

    private void InitializeDictionary()
    {
        var currentBinDateTime = LowerBound;
        while (currentBinDateTime <= UpperBound)
        {
            var nextBinDateTime = GetNextBinDateTime(currentBinDateTime);
            Data.Add(currentBinDateTime, default(T));
            currentBinDateTime = nextBinDateTime;
        }
    }

    private DateTime GetBinningDateTime(DateTime time)
    {
        return BinSize switch
        {
            DateTimeBinSize.Second => new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second),
            DateTimeBinSize.Minute => new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0),
            DateTimeBinSize.Hour => new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0),
            DateTimeBinSize.Day => new DateTime(time.Year, time.Month, time.Day, 0, 0, 0),
            DateTimeBinSize.Month => new DateTime(time.Year, time.Month, 1, 0, 0, 0),
            DateTimeBinSize.Year => new DateTime(time.Year, 1, 1, 0, 0, 0),
            _ => throw new ArgumentException("Bin size not supported")
        };
    }

    private DateTime GetNextBinDateTime(DateTime time)
    {
        return BinSize switch
        {
            DateTimeBinSize.Second => time.AddSeconds(1),
            DateTimeBinSize.Minute => time.AddMinutes(1),
            DateTimeBinSize.Hour => time.AddHours(1),
            DateTimeBinSize.Day => time.AddDays(1),
            DateTimeBinSize.Month => time.AddMonths(1),
            DateTimeBinSize.Year => time.AddYears(1),
            _ => throw new ArgumentException("Bin size not supported")
        };
    }
}

