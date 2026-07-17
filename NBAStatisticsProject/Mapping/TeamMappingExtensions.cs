using NBAStatisticsProject.Models;
using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Mapping
{
    public static class TeamMappingExtensions
    {
        public static IQueryable<TeamDto> ToDto(this IQueryable<Team> query) =>
       query.Select(t => new TeamDto(t.Id, t.Name, t.City, t.IsActive));
        public static TeamDto ToDto(this Team team) => 
            new TeamDto(team.Id, team.Name, team.City, team.IsActive);        
    }
}
