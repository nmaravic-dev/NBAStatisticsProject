
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
        string Season,
        int HomeTeamId,
        int AwayTeamId,
        int HomeScore,
        int AwayScore);
}

