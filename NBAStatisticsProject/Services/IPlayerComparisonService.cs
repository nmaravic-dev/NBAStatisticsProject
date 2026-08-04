using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IPlayerComparisonService
    {
        Task<ComparisonDto?> ComparePlayersAsync(int playerAId, int playerBId);
    }
}
