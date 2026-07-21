namespace NBAStatisticsProject.DTOs
{
    public record InjuryScoreDto(int PlayerId, string PlayerName, int InjuryCount, int TotalDaysInjured, int GamesMissed, double Score);
}
