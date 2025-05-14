namespace Sefer.Backend.Stats.Api.Endpoints;

public static class Students
{
    public static void MapStudentEndpoints(this WebApplication app)
    {
        app.MapGet("/new-students", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper, int? bin) =>
            await mediator.Send(new NewStudentsRequest(lower, upper, bin)));

        app.MapGet("/active-students", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper, int? bin) =>
            await mediator.Send(new ActiveStudentsRequest(lower, upper, bin)));

        app.MapGet("/student-age", [Authorize(Roles = "Admin")]
        async (IMediator mediator, int? courseId) =>
            await mediator.Send(new StudentAgeRequest { CourseId = courseId }));

        app.MapGet("/course-production", [Authorize(Roles = "Admin")]
        async (IMediator mediator, int? daysActive) =>
            await mediator.Send(new CourseProductionRequest(daysActive)));

        app.MapGet("/active-mentors", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper, int? bin) =>
            await mediator.Send(new ActiveMentorsRequest(lower, upper, bin)));

        app.MapGet("/dashboard", [Authorize(Roles = "Admin")]
        async (IMediator mediator, int? timezone) =>
            await mediator.Send(new DashboardStatsRequest(timezone ?? 0)));

        app.MapGet("/period-summary", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper) =>
            await mediator.Send(new PeriodSummaryStatsRequest(lower, upper)));
    }
}