using ResourceService.Models;

namespace ResourceService.Services.Interfaces
{
    public interface IResourceRepository
    {
        Task<List<Material>> GetAllMaterialsAsync();
        Task<List<Technique>> GetAllTechniquesAsync();
        Task<bool> DeleteMaterialAsync(Guid id);
        Task<bool> DeleteTechniqueAsync(Guid id);
        Task<Material?> GetMaterialByIdAsync(Guid id);
        Task<Technique?> GetTechniqueByIdAsync(Guid id);
        Task<Material?> GetMaterialByNameAsync(string name);
        Task<Technique?> GetTechniqueByNameAsync(string name);

        Task UpdateMaterialAsync(Material material);
        Task UpdateTechniqueAsync(Technique technique);
        Task AddMaterialAsync(Material material);
        Task AddTechniqueAsync(Technique technique);
    }
}