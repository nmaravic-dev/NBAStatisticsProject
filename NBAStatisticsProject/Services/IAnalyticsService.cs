using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IAnalyticsService
    {
        Task<List<StatsSummaryDto>> GetAllStatsSummaryAsync();
        Task<StatsSummaryDto?> GetPlayerSummaryAsync(int playerId);
    }
}
