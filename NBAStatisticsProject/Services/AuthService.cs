using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NBAStatisticsProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<AppUser> userManager, IConfiguration config, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }
        public async Task<AuthResponseDto?> LoginUserAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) 
            {
                _logger.LogWarning("Failed login for existing user {Email}", dto.Email);
                return null; 
            }                 

            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!valid || user == null) 
            {
                _logger.LogWarning("Failed login for existing user {Email}", dto.Email);
                return null; 
            }           

            var token = GenerateToken(user);
            return new AuthResponseDto(token, user.Email!);
        }

        public async Task<AuthResponseDto?> RegisterUserAsync(RegisterDto dto)
        {
            var user = new AppUser
            {
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return null;

            var token = GenerateToken(user);
            return new AuthResponseDto(token, user.Email);
        }

        private string GenerateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
