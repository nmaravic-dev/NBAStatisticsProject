using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Mapping;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Services
{
    public class TeamService : ITeamService
    {
        private readonly DataContext _context;
        public TeamService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<TeamDto>> GetAllAsync(bool includeInactive)
        {
            var query = _context.Teams.AsQueryable();
            if (!includeInactive)
            {
                query = query.Where(t => t.IsActive);
            }
            return await query.ToDto().ToListAsync();
        }

        public async Task<TeamDto?> GetByIdAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            return team?.ToDto();
        }

        public async Task<TeamDto> CreateAsync(TeamCreateDto dto)
        {
            var team = new Team
            {
                Name = dto.Name,
                City = dto.City,
                IsActive = true
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team.ToDto();
        }

        public async Task<List<TeamDto>> CreateManyAsync(List<TeamCreateDto> dtos)
        {
            var teams = dtos.Select(dto => new Team
            {
                Name = dto.Name,
                City = dto.City,
                IsActive = true
            }).ToList();
            _context.Teams.AddRange(teams);
            await _context.SaveChangesAsync();
            return teams.Select(t => t.ToDto()).ToList();
        }

        public async Task<TeamDto?> UpdateAsync(int id, TeamCreateDto dto)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return null;

            team.Name = dto.Name;
            team.City = dto.City;
            await _context.SaveChangesAsync();
            return team.ToDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return false;

            team.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
