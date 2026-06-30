using Domain.Models;

namespace Application.Abstractions;

public interface INodeRepository : IRepository<Node>
{
    Task<List<Node>> GetByGroupId(Guid groupId);
}