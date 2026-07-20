using System.ComponentModel.DataAnnotations;

namespace NBAStatisticsProject.DTOs
{
    public record PlayerGameStatDto(int Id, int PlayerId, string PlayerName, int GameId, DateTime GameDate, string HomeTeam, string AwayTeam, int Points,  int Assists, int Rebounds, int MinutesPlayed, int Steals, int Blocks, int Turnovers);
    public record PlayerGameStatCreateDto(int PlayerId, int GameId, [Range(0, int.MaxValue)] int Points, [Range(0, int.MaxValue)] int Rebounds, [Range(0, int.MaxValue)] int Assists, [Range(0, 48)] int MinutesPlayed, [Range(0, int.MaxValue)] int Steals, [Range(0, int.MaxValue)] int Blocks, [Range(0, int.MaxValue)] int Turnovers);

}

