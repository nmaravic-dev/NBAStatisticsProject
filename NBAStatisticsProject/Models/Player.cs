
namespace NBAStatisticsProject.Models

{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public ICollection<PlayerGameStat> GameStats { get; set; } = new List<PlayerGameStat>();
    }
}
