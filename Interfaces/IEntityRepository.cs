using KYC360.Models;
using Microsoft.AspNetCore.Mvc;

namespace KYC360.Interfaces
{
    public interface IEntityRepository
    {
        ICollection<Entity> GetEntities();
        Entity GetById(string id);

        Task UpdateEntity(Entity existingEntity, Entity updatedEntity);
        Task CreateEntity(Entity entity);
        Task DeleteEntity(Entity entity);
        ICollection<Entity> SearchEntities(string searchTerm);
        Task<IEnumerable<Entity>> GetEntitiesByPage(int page, int pageSize);
        Task<IEnumerable<Entity>> AdvancedSearch(string gender, DateTime? startDate, DateTime? endDate, List<string> countries);

    }
}
