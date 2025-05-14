namespace Sefer.Backend.Stats.Api.DataStructures;

public class Bin<TInterval, TQuantity>
{
    public TInterval? Interval { get; set; }

    public TQuantity? Quantity { get; set; }
}