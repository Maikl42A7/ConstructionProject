using Microsoft.EntityFrameworkCore;
using BidService.Models;

namespace BidService.Data
{
    public class BidDbContext : DbContext
    {
        public BidDbContext(DbContextOptions<BidDbContext> options)
            : base(options) { }

        public DbSet<Bid> Bids => Set<Bid>();

        public DbSet<RequiredMaterial> RequiredMaterials => Set<RequiredMaterial>();
        public DbSet<RequiredTechnique> RequiredTechniques => Set<RequiredTechnique>();
    }
}
