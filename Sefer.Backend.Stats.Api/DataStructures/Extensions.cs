namespace Sefer.Backend.Stats.Api.DataStructures;

public static class Extensions
{
    public static List<CategoryItemWeekly> CalculatePercentages(this List<CategoryItemWeekly> rawData)
    {
        var dictionary = new Dictionary<int, Dictionary<short, List<CategoryItemWeekly>>>();

        foreach (var data in rawData)
        {
            if (!dictionary.ContainsKey(data.Year)) dictionary.Add(data.Year, []);
            if (!dictionary[data.Year].ContainsKey(data.Week)) dictionary[data.Year].Add(data.Week, []);
            dictionary[data.Year][data.Week].Add(data);
        }

        foreach (var year in dictionary.Keys)
        {
            foreach (var week in dictionary[year].Values)
            {
                var total = week.Sum(data => data.Count);
                foreach (var data in week) { data.Percentage = 100d * data.Count / total; }
            }
        }

        return rawData;
    }
}