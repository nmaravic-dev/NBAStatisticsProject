
namespace NBAStatisticsProject.Models

{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public int Points { get; set; }
        public int Rebounds { get; set; }
        public int Assists { get; set; }
    }
}
