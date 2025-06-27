namespace BidService.Models
{
    public class RequiredMaterial
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BidId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}