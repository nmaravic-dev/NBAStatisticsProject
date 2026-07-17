namespace NBAStatisticsProject.DTOs
{
    public record TeamDto(int Id, string Name, string City, bool IsActive);
    public record TeamCreateDto(string Name, string City);
}
