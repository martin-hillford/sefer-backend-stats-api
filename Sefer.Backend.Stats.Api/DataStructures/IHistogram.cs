namespace Sefer.Backend.Stats.Api.DataStructures;

public interface IHistogram<TBinType,TValue>
{
    public List<Bin<TBinType,TValue>> Data { get; }
    
    public TValue Max { get; }
    
    public TValue Sum { get; }
}