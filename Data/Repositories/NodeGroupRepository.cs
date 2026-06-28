using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class NodeGroupRepository(AppDbContext context) : INodeGroupRepository
{
    public async Task AddAsync(NodeGroup entity)
    {
        await context.NodeGroups.AddAsync(entity);
    }

    public void Update(NodeGroup entity)
    {
        context.NodeGroups.Update(entity);
    }

    public void Delete(NodeGroup entity)
    {
        context.NodeGroups.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<NodeGroup?> GetByIdAsync(Guid id)
    {
        return await context.NodeGroups.FindAsync(id);
    }

    public async Task<List<NodeGroup>> GetAllAsync()
    {
        return await context.NodeGroups.ToListAsync();
    }
}