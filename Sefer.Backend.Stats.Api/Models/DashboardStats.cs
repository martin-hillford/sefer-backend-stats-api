// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class DashboardStats
{
    public long ActiveStudentsToday { get; set; }

    public long ActiveMentorsToday { get; set; }

    public long MessagesSendToday { get; set; }

    public long TotalCompletedCourses { get; set; }

    public long TotalLessonsSubmitted { get; set; }

    public long TotalStudents { get; set; }

    public long CurrentActiveStudents { get; set; }
}