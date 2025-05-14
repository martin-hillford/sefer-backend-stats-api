// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class DateTimeCount
{
    public long Count { get; set; }

    public DateTime Date { get; set; }
}

public class DateTimeValue
{
    public double Value { get; set; }

    public DateTime Date { get; set; }
}