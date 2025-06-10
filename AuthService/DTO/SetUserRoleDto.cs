namespace AuthService.DTO
{
    public class SetUserRoleDto
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
