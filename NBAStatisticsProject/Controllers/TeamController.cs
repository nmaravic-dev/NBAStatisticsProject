using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
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
            var teams = await _context.Teams.ToListAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeam(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, team);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddTeams(List<Team> teams)
        {
            _context.Teams.AddRange(teams);
            await _context.SaveChangesAsync();
            return Ok(teams);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, Team team)
        {
            var existingTeam = await _context.Teams.FindAsync(id);
            if(existingTeam == null)
                return NotFound();

            existingTeam.Name = team.Name;
            existingTeam.City = team.City;

            await _context.SaveChangesAsync();
            return Ok(existingTeam);
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
