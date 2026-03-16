using Microsoft.EntityFrameworkCore;
using ToDoList.Repository.Contracts.Common;
using ToDoList.Repository.Persistence;

namespace ToDoList.Repository.Implementations
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T>
        where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<bool> AddAsync(T? entity)
        {
            if (entity is null) return false;
            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null) return false;
            _dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<bool> UpdateAsync(T? entity)
        {
            if (entity is null) return false;
            _dbSet.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
