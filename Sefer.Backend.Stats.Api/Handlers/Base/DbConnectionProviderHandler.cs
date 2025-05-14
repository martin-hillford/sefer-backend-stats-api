namespace Sefer.Backend.Stats.Api.Handlers.Base;

public abstract class DbConnectionProviderHandler(IDbConnectionProvider connectionProvider)
{
    protected IDbConnection GetConnection() => connectionProvider.GetConnection();
}