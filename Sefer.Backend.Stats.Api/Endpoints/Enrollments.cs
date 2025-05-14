namespace Sefer.Backend.Stats.Api.Endpoints;

public static class Enrollments
{
    public static void MapEnrollmentEndpoints(this WebApplication app)
    {
        app.MapGet("/new-enrollments", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper, int? bin) =>
            await mediator.Send(new NewEnrollmentsRequest(lower, upper, bin)));

        app.MapGet("/completed-enrollments", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper, int? bin) =>
            await mediator.Send(new CompletedEnrollmentsRequest(lower, upper, bin)));

        app.MapGet("/submitted-lessons", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper, int? bin) =>
            await mediator.Send(new SubmittedLessonsRequest(lower, upper, bin)));

        app.MapGet("/enrollments", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper) =>
            await mediator.Send(new EnrollmentsStatsRequest(lower, upper)));
    }
}