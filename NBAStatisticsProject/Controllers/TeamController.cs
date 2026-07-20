using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {

        private readonly ITeamService _service;
        public TeamController(ITeamService service) => _service = service;


        [HttpGet]
        public async Task<IActionResult> GetAllTeams([FromQuery] bool includeInactive = false)
        {
            var teams = await _service.GetAllAsync(includeInactive);
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _service.GetByIdAsync(id);

            if (team == null)
                return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeam(TeamCreateDto teamDto)
        {
            var createdTeamDto = await _service.CreateAsync(teamDto);
            return CreatedAtAction(nameof(GetTeamById), new { id = createdTeamDto.Id }, createdTeamDto);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddTeams(List<TeamCreateDto> teamDtos)
        {
            var createdTeams = await _service.CreateManyAsync(teamDtos);
            return Ok(createdTeams);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, TeamCreateDto teamCreateDto)
        {
            var updatedTeamDto = await _service.UpdateAsync(id, teamCreateDto);
            if (updatedTeamDto == null)
                return NotFound();
            return Ok(updatedTeamDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
