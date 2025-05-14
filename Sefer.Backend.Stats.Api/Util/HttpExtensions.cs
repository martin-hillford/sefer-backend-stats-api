namespace Sefer.Backend.Stats.Api.Util;

public static class HttpExtensions
{
    public static void AddCacheHeaders(this HttpContext httpContext, int hours)
    {
        httpContext.Response.Headers.CacheControl = $"public,max-age={TimeSpan.FromHours(hours).TotalSeconds}";
    }
}