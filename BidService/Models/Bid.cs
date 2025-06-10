namespace BidService.Models
{
    public class Bid
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string userName { get; set; } = string.Empty;

        public string material { get; set; } = string.Empty;

        public string technique { get; set; } = string.Empty;

        public string result { get; set; } = string.Empty;
    }
}
