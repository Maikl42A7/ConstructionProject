using ConstructionProject.Contracts;
namespace BidService.DTO
{
    public class CreateBidDto
    {
        public string UserName { get; set; } = string.Empty;
        public List<RequiredItemDto> Materials { get; set; } = new();
        public List<string> Techniques { get; set; } = new();
    }
}
