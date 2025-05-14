namespace Sefer.Backend.Stats.Api.Endpoints;

public static class Performance
{
    public static void MapPerformanceEndpoints(this WebApplication app)
    {
        app.MapGet("/processing-time/histogram", EndpointUtils.GetAsync<ProcessingTimeRequest>);

        app.MapGet("/processing-time/weekly", EndpointUtils.GetAsync<ProcessingTimeWeeklyRequest>);

        app.MapGet("/processing-time", [Authorize(Roles = "Admin")]
        async (IMediator mediator, ushort hours) =>
            await mediator.Send(new RecentProcessingTimesRequest(hours)));

        app.MapGet("/api-requests/hour-of-the-week", EndpointUtils.GetAsync<ApiRequestPerHourRequest>);
    }
}