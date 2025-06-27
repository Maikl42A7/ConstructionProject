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
            try
            {
                await _userService.Register(userDto.userName, userDto.email, userDto.password);
                return Ok("Пользователь успешно зарегистрирован");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDtoLogin userDtoLogin)
        {
            try
            {
                var token = await _userService.Login(userDtoLogin.email, userDtoLogin.password);
                return Ok(new { token }); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var result = await _userService.SetUserRoleAsync(dto.UserId, dto.Role);
                if (!result)
                    return NotFound("Пользователь не найден");

                return Ok("Роль успешно обновлена");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}