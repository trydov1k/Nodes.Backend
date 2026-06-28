using Domain;
using Domain.DTOs.Nodes;

namespace Data.Services;

public interface INodeService
{
    Task<NodeDto> CreateAsync(CreateNodeDto dto);
    Task<NodeDto> UpdateAsync(Guid id, UpdateNodeDto dto);
    Task DeleteAsync(Guid id);
    Task<NodeDto?> GetByIdAsync(Guid id);
    Task<List<NodeDto>> GetAllAsync();

    Task<List<NodeDto>> GetByGroupId(Guid groupId);
}