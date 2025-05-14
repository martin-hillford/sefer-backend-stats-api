namespace Sefer.Backend.Stats.Api.Util;

public class SqlConnectionProvider(IConfiguration configuration) : IDbConnectionProvider
{
    public IDbConnection GetConnection()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var connectionString = configuration.GetSection("Database").GetValue<string>("ConnectionString");
        return new NpgsqlConnection(connectionString);
    }
}