namespace Sefer.Backend.Stats.Api.Requests;

public class CourseProductionRequest : IRequest<List<CourseProduction>>
{
    public int ActiveDays { get; set; } = 30;

    public CourseProductionRequest(int? daysActive)
    {
        if (daysActive.HasValue) ActiveDays = daysActive.Value;
    }
}