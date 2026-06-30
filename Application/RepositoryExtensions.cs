using Application.Abstractions;

namespace Application;

public static class RepositoryExtensions
{
    public static async Task<T> EnsureExistsAsync<T>(this IRepository<T> repository, Guid id) where T : class
    {
        var entity = await repository.GetByIdAsync(id);

        if (entity is null)
            throw new KeyNotFoundException($"{typeof(T).Name} {id} not found");
        
        return entity;
    }
}