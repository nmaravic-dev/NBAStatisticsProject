using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Mapping;
using Microsoft.EntityFrameworkCore;

namespace NBAStatisticsProject.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly DataContext _context;
        public AnalyticsService(DataContext context) => _context = context;
        public async Task<List<StatsSummaryDto>> GetAllStatsSummaryAsync()
        {
            return await _context.PlayerGameStats
                .ToStatsSummaryDto()
                .ToListAsync();
        }
        public async Task<StatsSummaryDto?> GetPlayerSummaryAsync(int playerId)
        {
            return await _context.PlayerGameStats
                .Where(pgs => pgs.PlayerId == playerId)
                .ToStatsSummaryDto()
                .FirstOrDefaultAsync();
        }
    }
}
