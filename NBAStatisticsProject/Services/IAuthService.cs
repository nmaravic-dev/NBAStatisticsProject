using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public interface IAuthService
    {
        public Task<AuthResponseDto?> RegisterUserAsync(RegisterDto dto);
        public Task<AuthResponseDto?> LoginUserAsync(LoginDto dto);

    }
}
