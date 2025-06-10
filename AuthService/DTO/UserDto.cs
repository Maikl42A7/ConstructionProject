using System.ComponentModel.DataAnnotations;

namespace AuthService.DTO
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        [Required]
        public string userName { get; set; } = string.Empty;
        [Required]
        public string email { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }
}
