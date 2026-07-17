namespace NBAStatisticsProject.DTOs
{
    public record PlayerDto(int Id, string Name, string Position, int TeamId, string TeamName);
    public record PlayerCreateDto(string Name, string Position, int TeamId);
}
