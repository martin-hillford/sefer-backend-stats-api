// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class ProcessingTimeWeekly
{
    public double Average { get; set; }

    public int Year { get; set; }

    public short Week { get; set; }

    public int Count { get; set; }
}