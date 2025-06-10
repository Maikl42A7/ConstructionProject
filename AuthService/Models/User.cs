namespace AuthService.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string userName { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string passwordHash { get; set; } = string.Empty;

        public string role { get; set; } = "New User";
    }
}
