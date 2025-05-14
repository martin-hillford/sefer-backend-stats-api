// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Models;

public class CategoryItemWeekly : CategoryItem
{
    public int Year { get; set; }

    public short Week { get; set; }
}