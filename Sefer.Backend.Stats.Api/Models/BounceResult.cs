namespace Sefer.Backend.Stats.Api.Models;

public class BounceResult(double percentage)
{
    public double BouncePercentage { get; private set; } = percentage;
}