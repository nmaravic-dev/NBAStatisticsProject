using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Mapping;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Services
{
    public class InjuryService : IInjuryService
    {
        private readonly DataContext _context;
        public InjuryService(DataContext context) => _context = context;

        public async Task<List<InjuryDto>> GetAllAsync()
        {
            return await _context.Injuries
                .ToDto()
                .ToListAsync();
        }

        public async Task<InjuryDto?> GetByIdAsync(int id)
        {
            return await _context.Injuries
                .Where(i => i.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
        }

        public async Task<InjuryDto?> CreateAsync(InjuryCreateDto dto)
        {
            if (!await _context.Players.AnyAsync(p => p.Id == dto.PlayerId))
                return null;

            if (dto.EndDate.HasValue && dto.EndDate < dto.StartDate)
                return null;

            var injury = new Injury()
            {
                PlayerId = dto.PlayerId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Severity = dto.Severity,
                Description = dto.Description
            };
            _context.Injuries.Add(injury);
            await _context.SaveChangesAsync();
            return await _context.Injuries
                .Where(i => i.Id == injury.Id)
                .ToDto()
                .FirstAsync();
        }

        public async Task<List<InjuryDto>> CreateManyAsync(List<InjuryCreateDto> dtos)
        {
            var injuries = dtos.Select(dto => new Injury()
            {
                PlayerId = dto.PlayerId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Severity = dto.Severity,
                Description = dto.Description
            }).ToList();
            _context.Injuries.AddRange(injuries);
            await _context.SaveChangesAsync();
            var ids = injuries.Select(i => i.Id).ToList();

            return await _context.Injuries
                .Where(i=> ids.Contains(i.Id))
                .ToDto()
                .ToListAsync();
        }

        public async Task<InjuryDto?> UpdateAsync(int id, InjuryDto dto)
        {
            if (!await _context.Players.AnyAsync(p => p.Id == dto.PlayerId))
                return null;

            if (dto.EndDate.HasValue && dto.EndDate < dto.StartDate)
                return null;

            var injury = await _context.Injuries.FindAsync(id);
            if (injury == null) return null;
            injury.PlayerId = dto.PlayerId;
            injury.StartDate = dto.StartDate;
            injury.EndDate = dto.EndDate;
            injury.Severity = dto.Severity;
            injury.Description = dto.Description;
            await _context.SaveChangesAsync();
            return await _context.Injuries
                .Where(i => i.Id == id)
                .ToDto()
                .FirstAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var injury = await _context.Injuries.FindAsync(id);
            if(injury == null) return false;
            _context.Injuries.Remove(injury);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
