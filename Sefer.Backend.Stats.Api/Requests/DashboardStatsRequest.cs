namespace Sefer.Backend.Stats.Api.Requests;

public class DashboardStatsRequest(int timezoneOffset) : IRequest<DashboardStats>
{
    public DateTime Today => DateTime.UtcNow.AddMinutes(timezoneOffset).Date;
}