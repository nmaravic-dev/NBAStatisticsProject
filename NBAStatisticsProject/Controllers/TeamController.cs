using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.Dtos;
using NBAStatisticsProject.Models;

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
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _context.Teams
                .Select(t => new TeamDto
                (
                    t.Id,
                    t.Name,
                    t.City
                ))
                .ToListAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _context.Teams
                .Where(t => t.Id == id)
                .Select(t => new TeamDto
                (
                    t.Id,
                    t.Name,
                    t.City
                ))
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
            var createdTeamDto = new TeamDto(team.Id, team.Name, team.City);
            return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, createdTeamDto);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddTeams(List<TeamCreateDto> teamDtos)
        {
            var teams = teamDtos.Select(teamDto => new Team
            {
                Name = teamDto.Name,
                City = teamDto.City
            }).ToList();
            _context.Teams.AddRange(teams);
            await _context.SaveChangesAsync();

            var createdTeams = teams
                .Select(t => new TeamDto(t.Id, t.Name, t.City))
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
            var updatedTeamDto = new TeamDto(existingTeam.Id, existingTeam.Name, existingTeam.City);
            return Ok(updatedTeamDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
