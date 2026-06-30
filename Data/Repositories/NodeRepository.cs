using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class NodeRepository(AppDbContext context) : INodeRepository
{
    public async Task AddAsync(Node entity)
    {
        await context.Nodes.AddAsync(entity);
    }

    public void Update(Node entity)
    {
        context.Nodes.Update(entity);
    }

    public void Delete(Node entity)
    {
        context.Nodes.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<Node?> GetByIdAsync(Guid id)
    {
        return await context.Nodes
            .Include(node => node.Group)
            .FirstOrDefaultAsync(node => node.NodeId == id);
    }

    public async Task<List<Node>> GetAllAsync()
    {
        return await context.Nodes
            .Include(node => node.Group)
            .ToListAsync();
    }

    public Task<List<Node>> GetByGroupId(Guid groupId)
    {
        return context.Nodes
            .Where(node => node.GroupId == groupId)
            .Include(node => node.Group)
            .ToListAsync();
    }
}