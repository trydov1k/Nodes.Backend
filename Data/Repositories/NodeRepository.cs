using Domain;

namespace Data.Repositories;

public class NodeRepository(AppDbContext context) : INodeRepository
{
    public Task AddAsync(Node entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Node entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Node entity)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Node?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Node>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}