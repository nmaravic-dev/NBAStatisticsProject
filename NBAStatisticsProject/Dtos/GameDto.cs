
using System.ComponentModel.DataAnnotations;

namespace NBAStatisticsProject.DTOs
{
    public record GameDto(
        int Id,
        DateTime Date,
        string Season,
        int HomeTeamId,
        string HomeTeamName,
        int AwayTeamId,
        string AwayTeamName,
        int HomeScore,
        int AwayScore);

    public record GameCreateDto(
        DateTime Date,
        [Required][StringLength(10)]
        string Season,
        [Range(1, int.MaxValue)]
        int HomeTeamId,
        [Range(1, int.MaxValue)]
        int AwayTeamId,
        [Range(0, int.MaxValue)]
        int HomeScore,
        [Range(0, int.MaxValue)]
        int AwayScore);
}

