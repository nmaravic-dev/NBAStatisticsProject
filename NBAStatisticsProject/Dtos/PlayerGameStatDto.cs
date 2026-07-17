namespace NBAStatisticsProject.Dtos
{
    public record PlayerGameStatDto(int Id, int PlayerId, string PlayerName, int GameId, DateTime GameDate, string HomeTeam, string AwayTeam, int Points,  int Assists, int Rebounds, int MinutesPlayed, int Steals, int Blocks, int Turnovers);
    public record PlayerGameStatCreateDto(int PlayerId, int GameId, int Points, int Rebounds, int Assists, int MinutesPlayed, int Steals, int Blocks, int Turnovers);

}

