using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private static readonly List<Player> players = new List<Player>(){
                new() { Id = 1, Name = "Jokic", Team = "Nuggets", Points = 25, Assists = 5, Rebounds = 10},
                new() { Id = 2, Name = "Doncic", Team = "Nuggets", Points = 30, Assists = 10, Rebounds = 12},
                new() { Id = 3, Name = "Bogdanovic", Team = "Bulls", Points = 22, Assists = 1, Rebounds = 30}
            };
        [HttpGet]
        public IActionResult GetAllPlayers()
        {
            return Ok(players);
        }
        [HttpGet("{id}")]
        public IActionResult GetPlayersById(int id)
        {
            var player = players.FirstOrDefault(p => p.Id == id);
            if (player == null)
                return NotFound();
            return Ok(player);
        }

        [HttpGet("team/{team}")]
        public IActionResult GetPlayersByTeam(string team)
        {
            var playersFromSameTeam = players.Where(p => p.Team == team).ToList();
            return Ok(playersFromSameTeam);
        }
        [HttpGet("max-points")]
        public IActionResult GetTopScorer()
        {
            var TopScorer = players.MaxBy(p => p.Points);
            return Ok(TopScorer);
        }

        [HttpGet("ordered-by-points")]

        public IActionResult OrderByPointsDesc()
        {
            var orderedListOfTopScorers = players.OrderByDescending(p => p.Points);
            return Ok(orderedListOfTopScorers);
        }
        [HttpGet("players-count")]

        public IActionResult NumberOfPlayers()
        {
            return Ok(players.Count());
        }
        [HttpPost]
        public IActionResult AddPlayer(Player player)
        {
            player.Id = players.Max(p => p.Id) + 1;
            players.Add(player);
            return Ok(players);
        }

        [HttpPut("{id}")]
        public IActionResult ChangePlayersStats(int id, [FromBody] Player updatedPlayer)
        {
            var existingPlayer = players.FirstOrDefault(p => p.Id == id);
            if (existingPlayer != null)
            {
                existingPlayer.Name = updatedPlayer.Name;
                existingPlayer.Team = updatedPlayer.Team;
                existingPlayer.Points = updatedPlayer.Points;
                existingPlayer.Assists = updatedPlayer.Assists;
                existingPlayer.Rebounds = updatedPlayer.Rebounds;
                return Ok(existingPlayer);
            }
            else
                return NotFound();
        }

        [HttpPatch("{id}")]

        public IActionResult ChangeIndividualStat(int id, [FromBody] JsonPatchDocument<Player> patchDoc)
        {
            var player = players.FirstOrDefault(p => p.Id == id);
            if (player == null) return NotFound();

            patchDoc.ApplyTo(player);
            return Ok(player);
        }
    }
}
