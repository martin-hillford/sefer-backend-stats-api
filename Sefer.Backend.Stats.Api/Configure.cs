namespace Sefer.Backend.Stats.Api;

public static class App
{
    public static WebApplication Create(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WithSharedConfig();
        builder.AddSwaggerWithToken();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddHostedService<QueryBackgroundService>();
        builder.Services.AddSingleton<IDbConnectionProvider, SqlConnectionProvider>();
        builder.Services.AddMemoryCache();
        builder.Services.AddTokenAuthentication();
        builder.AddCustomCorsMiddleware();

        var app = builder.Build();

        app.UseCustomCorsMiddleware();
        app.UseAuthorization();
        app.UseSwaggerWithToken();

        return app;
    }
}