using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Player> Players { get; set; }
    }
}
