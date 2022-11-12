namespace HealthEquity.Assessment.Domain.Repositories;

public interface IGenericRepository<T> where T : IAggregateRoot
{
    Task<T?> GetByIdAsync(long id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T> AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);

    Task Commit();
}
