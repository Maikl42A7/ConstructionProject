using AuthService.Models;
using AuthService.Services.Interfaces;

namespace AuthService.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminAsync(IUserRepository userRepository, IPasswordHasher hasher)
        {
            const string administratorEmail = "tormich991@gmail.com";

            var existing = await userRepository.GetByEmailAsync(administratorEmail);
            if (existing is not null) return;

            var administrator = new User
            {
                Id = Guid.NewGuid(),
                userName = "Maikl",
                email = administratorEmail,
                passwordHash = hasher.Generate("1234tryitout"), 
                role = "Администратор"
            };

            await userRepository.AddAsync(administrator);
        }
    }
}
