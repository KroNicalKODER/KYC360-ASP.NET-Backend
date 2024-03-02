using KYC360.Data;
using KYC360.Interfaces;
using KYC360.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KYC360.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly DataContext _context;
        public EntityRepository(DataContext context)
        {
            _context = context;
        }

        public Entity GetById(string id)
        {
            var entity = _context.Entities.Where(e => e.Id == id).FirstOrDefault();
            entity.Addresses = GetAddressByEntityId(id);
            entity.Dates = GetDatesByEntityId(id);
            entity.Names = GetNamesByEntityId(id);
            return entity;
        }

        public ICollection<Entity> GetEntities()
        {
            var entities = _context.Entities.OrderBy(e => e.Id).ToList();
            foreach (var entity in entities)
            {
                entity.Addresses = GetAddressByEntityId(entity.Id);
                entity.Names = GetNamesByEntityId(entity.Id);
                entity.Dates = GetDatesByEntityId(entity.Id);
            }
            return entities;
        }

        public async Task CreateEntity(Entity entity)
        {
            var addresses = entity.Addresses;
            var dates = entity.Dates;
            var names = entity.Names;
            entity.Addresses = null;
            entity.Dates = null;
            entity.Names = null;
            _context.Addresss.AddRange(addresses);
            _context.Dates.AddRange(dates);
            _context.Names.AddRange(names);

            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntity(Entity existingEntity, Entity updatedEntity)
        {
            // Update the main entity
            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);

            // Update child entities
            UpdateChildEntities(existingEntity.Addresses, updatedEntity.Addresses);
            UpdateChildEntities(existingEntity.Dates, updatedEntity.Dates);
            UpdateChildEntities(existingEntity.Names, updatedEntity.Names);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntity(Entity entity)
        {
            var addresses = entity.Addresses;
            var dates = entity.Dates;
            var names = entity.Names;
            _context.Addresss.RemoveRange(addresses);
            _context.Dates.RemoveRange(dates);
            _context.Names.RemoveRange(names);
            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entity>> GetEntitiesByPage(int page, int pageSize)
        {
            var entities = await _context.Entities
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var entity in entities)
            {
                entity.Addresses = GetAddressByEntityId(entity.Id);
                entity.Names = GetNamesByEntityId(entity.Id);
                entity.Dates = GetDatesByEntityId(entity.Id);
            }

            return entities;
        }

        public ICollection<Entity> SearchEntities(string searchTerm)
        {
            // Perform a case-insensitive search across multiple fields
            return _context.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Names)
                .Include(e => e.Dates)
                .Where(e =>
                    EF.Functions.Like(e.Addresses.Select(a => a.Country).FirstOrDefault(), $"%{searchTerm}%") ||
                    EF.Functions.Like(e.Addresses.Select(a => a.AddressLine).FirstOrDefault(), $"%{searchTerm}%") ||
                    EF.Functions.Like(e.Names.Select(n => n.FirstName).FirstOrDefault(), $"%{searchTerm}%") ||
                    EF.Functions.Like(e.Names.Select(n => n.MiddleName).FirstOrDefault(), $"%{searchTerm}%") ||
                    EF.Functions.Like(e.Names.Select(n => n.LastName).FirstOrDefault(), $"%{searchTerm}%"))
                .ToList();
        }

        public async Task<IEnumerable<Entity>> AdvancedSearch(string gender, DateTime? startDate, DateTime? endDate, List<string> countries)
        {
            var query = _context.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Names)
                .Include(e => e.Dates)
                .AsQueryable();

            // Apply gender filter
            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(e => e.Gender == gender);
            }

            // Apply date range filter
            if (startDate != null)
            {
                query = query.Where(e => e.Dates.Any(d => d.Dat >= startDate));
            }

            if (endDate != null)
            {
                query = query.Where(e => e.Dates.Any(d => d.Dat <= endDate));
            }

            // Apply country filter
            if (countries != null && countries.Any())
            {
                query = query.Where(e => e.Addresses.Any(a => countries.Contains(a.Country)));
            }

            return await query.ToListAsync();
        }

        private List<Address> GetAddressByEntityId(string entityId)
        {
            var addresses = _context.Addresss.Where(e => e.EntityId == entityId).ToList();

            return addresses;
        }

        private List<Name> GetNamesByEntityId(string entityId)
        {
            var names = _context.Names.Where(e => e.EntityId == entityId).ToList();

            return names;
        }

        private List<Date> GetDatesByEntityId(string entityId)
        {
            var dates = _context.Dates.Where(e => e.EntityId == entityId).ToList();

            return dates;
        }

        private void UpdateChildEntities<T>(ICollection<T> existingEntities, ICollection<T> updatedEntities)
            where T : class
        {
            // Remove entities that are not in the updated collection
            foreach (var existingEntity in existingEntities.ToList())
            {
                if (!updatedEntities.Any(e => ItemEquals(e, existingEntity)))
                {
                    _context.Entry(existingEntity).State = EntityState.Deleted;
                }
            }

            // Update or add entities in the updated collection
            foreach (var updatedEntity in updatedEntities)
            {
                var existingEntity = existingEntities.SingleOrDefault(e => ItemEquals(e, updatedEntity));

                if (existingEntity != null)
                {
                    // Update existing entity
                    _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                }
                else
                {
                    // Add new entity
                    existingEntities.Add(updatedEntity);
                }
            }
        }

        private bool ItemEquals<T>(T x, T y)
        {
            // Implement your equality check logic here based on your entity structure
            // For simplicity, assuming x and y have an Id property

            var xId = x.GetType().GetProperty("Id")?.GetValue(x);
            var yId = y.GetType().GetProperty("Id")?.GetValue(y);

            return xId != null && yId != null && xId.Equals(yId);
        }
    }
}
