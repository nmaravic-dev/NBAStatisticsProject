namespace NBAStatisticsProject.DTOs
{
    public record StatsSummaryDto(int PlayerId, int GameCount, string PlayerName, int TotalPoints, int TotalAssists, int TotalRebounds, double AveragePoints, double AverageAssists, double AverageRebounds);

}
