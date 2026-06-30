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

    public async Task<(List<Node> Items, int TotalCount)> GetByGroupId(Guid groupId, int page, int pageSize)
    {
        var query = context.Nodes
            .Where(node => node.GroupId == groupId)
            .Include(node => node.Group)
            .OrderBy(node => node.Header)
            .ThenBy(node => node.NodeId);
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    
    public async Task<(List<Node> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = context.Nodes
            .Include(node => node.Group)
            .OrderBy(node => node.Header)
            .ThenBy(node => node.NodeId);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}