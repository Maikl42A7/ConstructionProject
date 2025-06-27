namespace ResourceService.DTO
{
    public class CreateUpdateTechniqueDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public string Location { get; set; } = string.Empty;
    }
}
