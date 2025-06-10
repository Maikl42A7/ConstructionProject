using BidService.DTO;
using BidService.Models;
using BidService.Services.Interfaces;

namespace BidService.Services
{
    public class BidService
    {
        private readonly IBidRepository _repo;

        public BidService(IBidRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<BidDto>> GetAllAsync()
        {
            var bids = await _repo.GetAllAsync();
            return bids.Select(b => ToDto(b)).ToList();
        }

        public async Task<BidDto?> GetByIdAsync(Guid id)
        {
            var bid = await _repo.GetByIdAsync(id);
            return bid == null ? null : ToDto(bid);
        }

        public async Task<BidDto> CreateAsync(CreateBidDto dto)
        {
            var bid = new Bid
            {
                userName = dto.userName,
                material = dto.material,
                technique = dto.technique
            };

            await _repo.AddAsync(bid);
            return ToDto(bid);
        }

        public async Task<bool> UpdateAsync(BidDto dto)
        {
            var bid = await _repo.GetByIdAsync(dto.Id);
            if (bid == null) return false;

            bid.userName = dto.userName;
            bid.material = dto.material;
            bid.technique = dto.technique;
            bid.result = dto.result;
            return await _repo.UpdateAsync(bid);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }

        private static BidDto ToDto(Bid b) => new BidDto
        {
            Id = b.Id,
            userName = b.userName,
            material = b.material,
            technique = b.technique,
            result = b.result
        };
    }

}
