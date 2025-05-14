namespace Sefer.Backend.Stats.Api.Endpoints;

public static class UserDevices
{
    public static void MapUserDeviceEndpoints(this WebApplication app)
    {
        app.MapGet("/resolutions", EndpointUtils.GetAsync<ResolutionRequest>);
        app.MapGet("/browsers/weekly", EndpointUtils.GetAsync<BrowserClassWeeklyRequest>);
        app.MapGet("/browsers", EndpointUtils.GetAsync<BrowserClassRequest>);
        app.MapGet("/operating-systems/weekly", EndpointUtils.GetAsync<OperatingSystemWeeklyRequest>);
        app.MapGet("/operating-systems", EndpointUtils.GetAsync<OperatingSystemRequest>);
    }
}