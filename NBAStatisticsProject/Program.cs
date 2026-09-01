using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.Models;
using NBAStatisticsProject.Services;
using Scalar.AspNetCore;
using System.Text;
namespace NBAStatisticsProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IPlayerGameStatService, PlayerGameStatService>();
            builder.Services.AddScoped<IInjuryService, InjuryService>();
            builder.Services.AddScoped<IInjuryScoreService, InjuryScoreService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IWatchlistService, WatchlistService>();
            builder.Services.AddScoped<IPlayerComparisonService, PlayerComparisonService>();
            builder.Services.AddProblemDetails();
            builder.Services.AddIdentityCore<AppUser>()
                .AddEntityFrameworkStores<DataContext>();
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<DataContext>(tags: new[] { "ready" });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            var allowedOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseExceptionHandler(_ => { });

            var forwardedOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            };
            forwardedOptions.KnownNetworks.Clear();
            forwardedOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardedOptions);

            // Apply pending migrations on startup (creates tables on the production DB)
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                db.Database.Migrate();
            }
            // Configure the HTTP request pipeline.

            app.UseCors("Frontend");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapOpenApi();
            app.MapScalarApiReference();
            app.MapControllers();
            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = _ => false
            });
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready")
            });

            app.Run();
        }
    }
}
