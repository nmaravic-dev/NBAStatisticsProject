using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.Services;
using Scalar.AspNetCore;
namespace NBAStatisticsProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IPlayerGameStatService, PlayerGameStatService>();
            builder.Services.AddScoped<IInjuryService, InjuryService>();
            builder.Services.AddScoped<IInjuryScoreService, InjuryScoreService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
