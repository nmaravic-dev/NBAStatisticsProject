using System.ComponentModel.DataAnnotations;

namespace NBAStatisticsProject.DTOs
{
    public record TeamDto(int Id, string Name, string City, bool IsActive);
    public record TeamCreateDto([Required][StringLength(50)] string Name,[Required][StringLength(50)] string City);
}
