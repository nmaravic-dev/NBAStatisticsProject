using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface ITeamService
    {
        Task<List<TeamDto>> GetAllAsync(bool includeInactive);
        Task<TeamDto?> GetByIdAsync(int id);
        Task<TeamDto> CreateAsync(TeamCreateDto dto);
        Task<List<TeamDto>> CreateManyAsync(List<TeamCreateDto> dtos);
        Task<TeamDto?> UpdateAsync(int id, TeamCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
