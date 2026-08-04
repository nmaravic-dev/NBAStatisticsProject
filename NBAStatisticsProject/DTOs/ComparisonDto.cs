namespace NBAStatisticsProject.DTOs
{
    public record ComparisonDto(PlayerComparisonDto PlayerA, PlayerComparisonDto PlayerB);
    public record PlayerComparisonDto(int PlayerId, string PlayerName, int GamesPlayed, double PointsPerGame, double AssistsPerGame, double ReboundsPerGame, double MinutesPlayedPerGame, double StealsPerGame, double BlocksPerGame, double TurnoversPerGame);
}
