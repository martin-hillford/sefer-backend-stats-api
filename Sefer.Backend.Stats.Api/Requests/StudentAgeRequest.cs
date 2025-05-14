// ReSharper disable PropertyCanBeMadeInitOnly.Global, UnusedAutoPropertyAccessor.Global
namespace Sefer.Backend.Stats.Api.Requests;

public class StudentAgeRequest : IRequest<Histogram>
{
    public int? CourseId { get; set; }
}