namespace ResourceService.Models
{
    public class Technique
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public string Location { get; set; } = string.Empty;
    }
}
