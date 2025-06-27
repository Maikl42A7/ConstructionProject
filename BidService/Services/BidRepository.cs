using BidService.Models;
using BidService.Data;
using BidService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BidService.Services
{
    public class BidRepository : IBidRepository
    {
        private readonly BidDbContext _context;

        public BidRepository(BidDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetAllAsync() =>
            await _context.Bids
                .Include(b => b.RequiredMaterials)
                .Include(b => b.RequiredTechniques)
                .ToListAsync();

        public async Task<Bid?> GetByIdAsync(Guid id) =>
            await _context.Bids
                .Include(b => b.RequiredMaterials)
                .Include(b => b.RequiredTechniques)
                .FirstOrDefaultAsync(b => b.Id == id); 

        public async Task AddAsync(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Bid bid)
        {
            _context.Bids.Update(bid);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if (bid is null) return false;

            _context.Bids.Remove(bid);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task ClearAllAsync()
        {
            await _context.Bids.ExecuteDeleteAsync();
        }
    }
}
