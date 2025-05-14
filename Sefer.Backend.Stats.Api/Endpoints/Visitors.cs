namespace Sefer.Backend.Stats.Api.Endpoints;

public static class Visitors
{
    public static void MapVisitorEndpoints(this WebApplication app)
    {
        app.MapGet("/blog-visitors", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper, int? bin) =>
            await mediator.Send(new VisitsRequest(lower, upper, bin, "/blogs/%")));

        app.MapPost("/visitors", [Authorize(Roles = "Admin")]
        async (IMediator mediator, [FromBody] VisitsModel body) =>
            await mediator.Send(new VisitsRequest(body)));

        app.MapGet("/bounce-percentage", [Authorize(Roles = "Admin")]
        async (IMediator mediator) => await mediator.Send(new BouncePercentageRequest()));

        app.MapPost("/bounce-percentage", [Authorize(Roles = "Admin")]
        async (IMediator mediator, [FromBody] BouncePercentageRequest request) =>
            await mediator.Send(request));

        app.MapGet("/homepage-visits", [Authorize(Roles = "Admin")]
        async (IMediator mediator, long? lower, long? upper, int? bin) =>
            await mediator.Send(new HomePageVisitsRequest(lower, upper, bin)));
    }
}