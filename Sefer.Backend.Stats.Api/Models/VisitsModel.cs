// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class VisitsModel
{
    public long Lower { get; set; }
    
    public long Upper { get; set; }
    
    public DateTimeBinSize BinSize { get; set; }
    
    public string? Path { get; set; }
}