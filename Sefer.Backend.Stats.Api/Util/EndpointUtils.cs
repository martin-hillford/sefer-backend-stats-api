namespace Sefer.Backend.Stats.Api.Util;

public static class EndpointUtils
{
    [Authorize(Roles = "Admin")]
    public static async Task<object?> GetAsync<TRequest>(HttpContext http, IMediator mediator) where TRequest : IBaseRequest, new()
    {
        http.AddCacheHeaders(8);
        var request = new TRequest();
        return await mediator.Send(request);
    }
}