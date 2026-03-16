namespace ToDoList.Repository.Contracts.Common
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<bool> AddAsync(T? entity);
        public Task<bool> UpdateAsync(T? entity);
        public Task<T?> GetByIdAsync(int id);
        public Task<IReadOnlyCollection<T>> GetAllAsync();
        public Task<bool> DeleteAsync(int id);
    }
}
