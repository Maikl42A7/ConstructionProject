namespace AuthService.Models
{
    public class Roles
    {
        public const string Administrator = "Администратор";
        public const string ManagerOrders = "Менеджер по заявкам";
        public const string ManagerResources = "Менеджер по ресурсам";

        public static readonly string[] All =
        {
            Administrator,
            ManagerOrders,
            ManagerResources
        };
    }
}
