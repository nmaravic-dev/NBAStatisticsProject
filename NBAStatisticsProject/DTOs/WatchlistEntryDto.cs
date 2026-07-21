namespace NBAStatisticsProject.DTOs
{
    public record WatchlistEntryDto(int Id, int PlayerId, string PlayerName, string Position, string TeamName);
    public record WatchlistEntryCreateDto(int PlayerId);
}
