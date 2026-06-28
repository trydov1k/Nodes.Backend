using Domain;

namespace Data.Repositories;

public class NodeGroupRepository(AppDbContext context) : INodeGroupRepository
{
    public Task AddAsync(NodeGroup entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(NodeGroup entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(NodeGroup entity)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<NodeGroup?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<NodeGroup>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}