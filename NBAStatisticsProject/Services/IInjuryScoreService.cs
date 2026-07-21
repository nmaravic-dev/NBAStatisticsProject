using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IInjuryScoreService
    {
        public Task<InjuryScoreDto?> GetInjuryScoreAsync(int playerId);
        Task<List<InjuryScoreDto>> GetAllInjuryScoresAsync();
    }
}
