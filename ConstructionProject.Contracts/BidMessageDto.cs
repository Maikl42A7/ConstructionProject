namespace ConstructionProject.Contracts
{
    public class BidMessageDto
    {
        public Guid BidId { get; set; }

        public List<RequiredItemDto> RequiredMaterials { get; set; } = new();
        public List<string> RequiredTechniques { get; set; } = new();
    }
}
