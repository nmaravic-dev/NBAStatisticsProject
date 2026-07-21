using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IWatchlistService
    {
        public Task<List<WatchlistEntryDto>> GetAllAsync(string userId);
        public Task<WatchlistEntryDto?> CreateAsync(string userId, WatchlistEntryCreateDto dto);
        public Task<bool> DeleteAsync(string userId, int id);
    }
}
