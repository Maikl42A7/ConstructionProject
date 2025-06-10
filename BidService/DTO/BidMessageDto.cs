namespace BidService.DTO
{
    public class BidMessageDto
    {
        public Guid BidId { get; set; }
        public string material { get; set; } = string.Empty;
        public string technique { get; set; } = string.Empty;
    }
}
