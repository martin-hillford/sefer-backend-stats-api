// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class CourseProduction
{
    public int Id { get; set; }
    
    public string? Name { get; set; }

    public long Done { get; set; }

    public long Cancelled { get; set; }
    
    public long InActive { get; set; }
    
    public long Active { get; set; }
}