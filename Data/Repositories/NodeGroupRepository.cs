using Application.Abstractions;
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

    public async Task<(List<NodeGroup> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = context.NodeGroups
            .OrderBy(group => group.Name)
            .ThenBy(group => group.GroupId);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}