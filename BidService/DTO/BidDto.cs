namespace BidService.DTO
{
    public class BidDto
    {
        public Guid Id { get; set; }
        public string userName { get; set; } = string.Empty;
        public string material { get; set; } = string.Empty;
        public string technique { get; set; } = string.Empty;
        public string result { get; set; } = string.Empty;
    }
}
