namespace NBAStatisticsProject.Models
{
    public class PlayerGameStat
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player? Player { get; set; } = null!;
        public int GameId { get; set; }
        public Game? Game { get; set; } = null!;
        public int Points { get; set; }
        public int Rebounds { get; set; }
        public int Assists { get; set; }
        public int MinutesPlayed { get; set; }
        public int Steals { get; set; }
        public int Blocks { get; set; }
        public int Turnovers { get; set; }
    }
}
