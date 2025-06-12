namespace Sefer.Backend.Stats.Api.Services;

public class QueryBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IDbConnectionProvider _connectionProvider = serviceProvider.GetRequiredService<IDbConnectionProvider>();

    private readonly IMemoryCache _cache = serviceProvider.GetRequiredService<IMemoryCache>();

    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

    private Timer? _timer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // At boot first initialize the cache
        await DoWorkAsync();

        // Start the time for the service
        var nextRun = DateTime.Today.AddDays(1).ToUniversalTime().AddHours(5);
        var dueTime = nextRun - DateTime.UtcNow;
        _timer = new Timer(DoWork, null, dueTime, TimeSpan.FromHours(24));
    }

    private void DoWork(object? state)
    {
        Task.Run(DoWorkAsync);
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public override void Dispose() => _timer?.Dispose();

    private async Task DoWorkAsync()
    {
        // First update the clients visits table
        UpdateClientVisitsTable();

        // Run several long queries.
        // They will be cached for a next run of this background service
        await RefreshBouncePercentageCaching();
        await RefreshOperatingSystemCaching();
        await RefreshOperatingSystemWeeklyCaching();
        await RefreshBrowserClassCaching();
        await RefreshBrowserWeeklyClassCaching();
        await RefreshResolutionsClassCaching();
        await RefreshProcessingTimeCaching();
        await RefreshProcessingTimeWeeklyCaching();
        await ApiRequestPerHourCaching();
    }

    private void UpdateClientVisitsTable()
    {
        var query = DatabaseRequestHandler.GetQuery("update_client_visits");
        using var connection = _connectionProvider.GetConnection();
        connection.Execute(query, null, null, 3600);
    }

    private async Task RefreshBouncePercentageCaching()
    {
        var request = new BouncePercentageRequest();
        _cache.Remove(request.GetQueryKey());
        await _mediator.Send(request);
    }

    private Task RefreshOperatingSystemCaching() =>
        Refresh<OperatingSystemRequest>(OperatingSystemHandler.CacheKey);

    private Task RefreshOperatingSystemWeeklyCaching() =>
        Refresh<OperatingSystemWeeklyRequest>(OperatingSystemWeeklyHandler.CacheKey);

    private Task RefreshBrowserClassCaching() =>
        Refresh<BrowserClassRequest>(BrowserClassHandler.CacheKey);

    private Task RefreshBrowserWeeklyClassCaching() =>
        Refresh<BrowserClassWeeklyRequest>(BrowserClassWeeklyHandler.CacheKey);

    private Task RefreshResolutionsClassCaching() =>
        Refresh<ResolutionRequest>(ResolutionHandler.CacheKey);

    private Task RefreshProcessingTimeCaching() =>
        Refresh<ProcessingTimeRequest>(ProcessingTimeHandler.CacheKey);

    private Task RefreshProcessingTimeWeeklyCaching() =>
        Refresh<ProcessingTimeWeeklyRequest>(ProcessingTimeWeeklyHandler.CacheKey);

    private Task ApiRequestPerHourCaching() =>
        Refresh<ApiRequestPerHourRequest>(ApiRequestPerHourHandler.CacheKey);

    private async Task Refresh<T>(string cacheKey) where T : IBaseRequest, new()
    {
        var request = new T();
        _cache.Remove(cacheKey);
        await _mediator.Send(request);
    }
}