using Microsoft.EntityFrameworkCore;
using ResourceService.Data;
using ResourceService.Models;
using ResourceService.Services.Interfaces;

namespace ResourceService.Services
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ResourceDbContext _context;

        public ResourceRepository(ResourceDbContext context)
        {
            _context = context;
        }
        public async Task<List<Material>> GetAllMaterialsAsync() => await _context.Materials.ToListAsync();
        public async Task<List<Technique>> GetAllTechniquesAsync() => await _context.Techniques.ToListAsync();

        public async Task AddMaterialAsync(Material material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
        }

        public async Task AddTechniqueAsync(Technique technique)
        {
            _context.Techniques.Add(technique);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMaterialAsync(Guid id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return false;
            _context.Materials.Remove(material);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTechniqueAsync(Guid id)
        {
            var technique = await _context.Techniques.FindAsync(id);
            if (technique == null) return false;
            _context.Techniques.Remove(technique);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Material?> GetMaterialByNameAsync(string name) =>
            await _context.Materials.FirstOrDefaultAsync(m => m.Name.ToLower() == name.ToLower());

        public async Task<Technique?> GetTechniqueByNameAsync(string name) =>
            await _context.Techniques.FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());

        public async Task UpdateMaterialAsync(Material material)
        {
            _context.Materials.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTechniqueAsync(Technique technique)
        {
            _context.Techniques.Update(technique);
            await _context.SaveChangesAsync();
        }

        public async Task<Material?> GetMaterialByIdAsync(Guid id)
        {
            return await _context.Materials.FindAsync(id);
        }

        public async Task<Technique?> GetTechniqueByIdAsync(Guid id)
        {
            return await _context.Techniques.FindAsync(id);
        }
    }
}