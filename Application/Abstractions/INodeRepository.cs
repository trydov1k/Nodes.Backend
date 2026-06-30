using Domain.Models;

namespace Application.Abstractions;

public interface INodeRepository : IRepository<Node>
{
    Task<(List<Node> Items, int TotalCount)> GetByGroupId(Guid groupId, int page, int pageSize);
    Task<(List<Node> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
}