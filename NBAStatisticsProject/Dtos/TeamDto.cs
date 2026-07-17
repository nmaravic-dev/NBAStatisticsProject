namespace NBAStatisticsProject.Dtos
{
    public record TeamDto(int Id, string Name, string City);
    public record TeamCreateDto(string Name, string City);
}
