namespace OnlineMarket.Infrastructure.Interfaces
{
    public interface IBaseRepository<T> : IDisposable
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();

    }
}
