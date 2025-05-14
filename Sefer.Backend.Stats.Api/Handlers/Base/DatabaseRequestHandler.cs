namespace Sefer.Backend.Stats.Api.Handlers.Base;

public abstract class DatabaseRequestHandler
{
    protected readonly IDbConnection DbConnection;

    protected DatabaseRequestHandler(IDbConnectionProvider provider)
    {
        DbConnection = provider.GetConnection();
    }
    
    protected DatabaseRequestHandler(IServiceProvider provider)
    {
        DbConnection = provider.GetRequiredService<IDbConnectionProvider>().GetConnection();
    }

    public static string GetQuery(string name)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Sefer.Backend.Stats.Api.Queries.{name}.sql";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) throw new FileNotFoundException($"Resource not found: {resourceName}");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        catch (Exception exp)
        {
            throw new Exception($"Query '{name}' could not be found", exp);
        }
    }
}