namespace Application.Abstractions;

public interface IRepository<T>
    where T : class
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
    
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync(); 
}