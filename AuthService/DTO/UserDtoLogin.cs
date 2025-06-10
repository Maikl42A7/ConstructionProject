using System.ComponentModel.DataAnnotations;

namespace AuthService.DTO
{
    public class UserDtoLogin
    {
        [Required]
        public string email { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }
}
