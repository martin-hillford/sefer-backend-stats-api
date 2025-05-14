
namespace Sefer.Backend.Stats.Api.Handlers.Base;

public abstract class CachedRequestHandler<TRequest, TResponse>(IServiceProvider provider)
    : DatabaseRequestHandler(provider), IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IMemoryCache _cache = provider.GetRequiredService<IMemoryCache>();

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var key = GetCacheKey(request);
        var cached = _cache.Get<TResponse>(key);
        if (cached != null) return cached;

        var data = await GetData(request, cancellationToken);
        // Note: since we have a background task that takes care of caching.
        // make the caching just a bit longer then interval of the background service
        // to prevent the cache is just expired before running the service
        _cache.Set(key, data, DateTimeOffset.UtcNow.AddDays(1).AddHours(1));
        return data;
    }

    protected abstract string GetCacheKey(TRequest request);

    protected abstract Task<TResponse> GetData(TRequest request, CancellationToken cancellationToken);
}