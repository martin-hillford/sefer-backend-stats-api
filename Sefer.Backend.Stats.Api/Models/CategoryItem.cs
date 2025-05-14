// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class CategoryItem
{
    private string? _name;

    public int Count { get; set; }

    public string? Name
    {
        get => _name ?? "Unknown";
        set => _name = value;
    }

    public double Percentage { get; set; }
}