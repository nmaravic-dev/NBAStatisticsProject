using System.ComponentModel.DataAnnotations;

namespace NBAStatisticsProject.DTOs
{
    public record PlayerDto(int Id, string FirstName, string LastName, string Position, int TeamId, string TeamName);
    public record PlayerCreateDto([Required][StringLength(50)] string FirstName, [Required][StringLength(25)] string LastName, [Required][StringLength(25)] string Position, [Range(1, int.MaxValue)] int TeamId);
}
