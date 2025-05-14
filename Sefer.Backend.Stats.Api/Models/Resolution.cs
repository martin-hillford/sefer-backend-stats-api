// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class Resolution
{
    public string? Name { get; set; }

    public long Count { get; set; }

    public long TotalCount { get; set; }

    public double Percentage { get; set; }

    public int? MinWidth { get; set; }

    public int? MaxWidth { get; set; }
}