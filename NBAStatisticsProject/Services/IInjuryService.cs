using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IInjuryService
    {
        Task<List<InjuryDto>> GetAllAsync();
        Task<InjuryDto?> GetByIdAsync(int id);
        Task<InjuryDto?> CreateAsync(InjuryCreateDto dto);
        Task<List<InjuryDto>> CreateManyAsync(List<InjuryCreateDto> dtos);
        Task<InjuryDto?> UpdateAsync(int id, InjuryCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
