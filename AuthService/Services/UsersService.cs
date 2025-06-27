using AuthService.Services.Interfaces;
using AuthService.Jwt;
using AuthService.Models;

namespace AuthService.Services
{
    public class UsersService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepository _userRepository;

        public UsersService(IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider,
            IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<bool> SetUserRoleAsync(Guid userId, string role)
        {
            if (!Roles.All.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Недопустимая роль");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.role = role.ToLower();
            return await _userRepository.UpdateAsync(user);
        }

        public async Task Register(string userName, string email, string password)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser is not null)
            {
                throw new Exception("Пользователь с таким email уже существует");
            }

            var hashedPassword = _passwordHasher.Generate(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                userName = userName,
                email = email,
                passwordHash = hashedPassword,
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                throw new Exception("Неверный email или пароль");
            }

            var result = _passwordHasher.Verify(password, user.passwordHash);

            if (result == false)
            {
                throw new Exception("Неверный email или пароль");
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }
    }
}

