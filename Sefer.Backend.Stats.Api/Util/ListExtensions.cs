namespace Sefer.Backend.Stats.Api.Util;

public static class ListExtensions
{
    public static double Percentile(this List<double> list, int k)
    {
        if(list.Count != 0 == false) throw new IndexOutOfRangeException("It is not possible to determine a percentile for an empty list");

        var ordered = list.OrderBy(l => l).ToList();
        var index = (double)k / 100 * ordered.Count;

        // The corresponding value in your data set is the k-th percentile.
        var kIndex = (int) Math.Round(index);
        return kIndex == 0 ? ordered[0] : ordered[kIndex -1];
    }
    
    // ReSharper disable once UnusedMember.Global
    public static double Median(this List<double> list)
    {
        if(list.Count != 0 == false) throw new IndexOutOfRangeException("It is not possible to determine a percentile for an empty list");
        if (list.Count == 1) return list[0];

        var ordered = list.OrderBy(l => l).ToList();
        var index = 0.5 * ordered.Count;
        var kIndex = (int)Math.Ceiling(index);

        if (index.Equals(kIndex) == false) return ordered[kIndex -1];
        return 0.5 * ordered[kIndex] + ordered[kIndex - 1] * 0.5;
    }
}