// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class PeriodSummaryStats
{
    public long ActiveStudents { get; set; }
    
    public long NewStudents { get; set; }
    
    public long SubmittedLessons { get; set; }
    
    public long CompletedCourses { get; set; }
    
    public long ClosedCourses { get; set; }
    
    public long NewEnrollments { get; set; }
    
    public long Messages { get; set;}
    
    public double AverageReviewTime { get; set;}
}