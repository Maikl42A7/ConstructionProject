namespace BidService.Models
{
    public class Bid
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserName { get; set; } = string.Empty;

        public string Result { get; set; } = "В обработке";

        public ICollection<RequiredMaterial> RequiredMaterials { get; set; } = new List<RequiredMaterial>();
        public ICollection<RequiredTechnique> RequiredTechniques { get; set; } = new List<RequiredTechnique>();
    }
}
