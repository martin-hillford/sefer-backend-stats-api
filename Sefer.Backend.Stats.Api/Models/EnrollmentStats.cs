// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
namespace Sefer.Backend.Stats.Api.Models;

public class EnrollmentStats
{
    public long Total { get; set; }
    
    public long Completed { get; set; }
    
    public long Open => Total - Completed - Closed;
    
    public long Closed { get; set; }

    public long Males { get; set; }

    public long Females { get; set; }    
}