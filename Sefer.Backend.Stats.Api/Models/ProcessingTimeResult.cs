// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class ProcessingTimeResult
{
    public short Bin { get; set; }

    public double ProcessingTime { get; set; }

    public long Count { get; set; }
}