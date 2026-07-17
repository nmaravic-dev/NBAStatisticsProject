using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Models;
using NBAStatisticsProject.Mapping;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        
        private readonly DataContext _context;
        public TeamController(DataContext context) 
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllTeams([FromQuery] bool includeInactive = false)
        {
            var query = _context.Teams.AsQueryable();

            if(!includeInactive)            
                query = query.Where(t => t.IsActive);
            
            var teams = await query
                .ToDto()
                .ToListAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _context.Teams
                .Where(t => t.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
            if (team == null)
                return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeam(TeamCreateDto teamDto)
        {
            var team = new Team
            {
                Name = teamDto.Name,
                City = teamDto.City
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            var createdTeamDto = team.ToDto();
            return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, createdTeamDto);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddTeams(List<TeamCreateDto> teamDtos)
        {
            var teams = teamDtos.Select(teamDto => new Team
            {
                Name = teamDto.Name,
                City = teamDto.City,
            }).ToList();
            _context.Teams.AddRange(teams);
            await _context.SaveChangesAsync();

            var createdTeams = teams
                .Select(t => t.ToDto())
                .ToList();

            return Ok(createdTeams);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, TeamCreateDto teamCreateDto)
        {
            var existingTeam = await _context.Teams.FindAsync(id);
            if(existingTeam == null)
                return NotFound();

            existingTeam.Name = teamCreateDto.Name;
            existingTeam.City = teamCreateDto.City;
            await _context.SaveChangesAsync();
            var updatedTeamDto = existingTeam.ToDto();
            return Ok(updatedTeamDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();
            team.IsActive = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
