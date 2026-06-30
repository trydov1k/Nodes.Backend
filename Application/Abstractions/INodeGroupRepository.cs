using Domain.Models;

namespace Application.Abstractions;

public interface INodeGroupRepository : IRepository<NodeGroup>
{
    Task<(List<NodeGroup> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
}