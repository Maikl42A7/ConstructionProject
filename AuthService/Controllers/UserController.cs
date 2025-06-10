using AuthService.Data;
using AuthService.DTO;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UsersService _userService;

        public UserController(UsersService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            await _userService.Register(userDto.userName, userDto.email, userDto.password);

            return Ok("Пользователь зарегистрирован");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDtoLogin userDtoLogin) 
        {
            var token = await _userService.Login(userDtoLogin.email, userDtoLogin.password);

            return Ok(token);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("set-role")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> SetUserRole(SetUserRoleDto dto)
        {
            var result = await _userService.SetUserRoleAsync(dto.UserId, dto.Role);
            if (!result)
                return NotFound("Пользователь не найден");

            return Ok("Роль обновлена");
        }
    }
}

