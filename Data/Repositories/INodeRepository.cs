using Domain;
using Domain.Models;

namespace Data.Repositories;

public interface INodeRepository : IRepository<Node>
{
    Task<List<Node>> GetByGroupId(Guid groupId);
}