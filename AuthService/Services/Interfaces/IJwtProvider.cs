using AuthService.Models;

namespace AuthService.Services.Interfaces
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}
