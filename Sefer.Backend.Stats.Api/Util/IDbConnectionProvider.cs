namespace Sefer.Backend.Stats.Api.Util;

public interface IDbConnectionProvider
{
    IDbConnection GetConnection();
}