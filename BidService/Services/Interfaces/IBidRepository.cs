using BidService.Models;

namespace BidService.Services.Interfaces
{
    public interface IBidRepository
    {
        Task<List<Bid>> GetAllAsync();
        Task<Bid?> GetByIdAsync(Guid id);
        Task AddAsync(Bid bid);
        Task<bool> UpdateAsync(Bid bid);
        Task<bool> DeleteAsync(Guid id);
        Task ClearAllAsync();
    }
}
