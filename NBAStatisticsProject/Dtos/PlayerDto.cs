using System.ComponentModel.DataAnnotations;

namespace NBAStatisticsProject.DTOs
{
    public record PlayerDto(int Id, string Name, string Position, int TeamId, string TeamName);
    public record PlayerCreateDto([Required][StringLength(50)] string Name, [Required][StringLength(25)] string Position, [Range(1, int.MaxValue)] int TeamId);
}
