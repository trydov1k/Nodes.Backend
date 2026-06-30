using Domain.DTOs.NodeGroups;

namespace Application.Services;

public interface INodeGroupService
{
    Task<NodeGroupDto> CreateAsync(CreateNodeGroupDto dto);
    Task<NodeGroupDto> UpdateAsync(Guid id, UpdateNodeGroupDto dto);
    Task DeleteAsync(Guid id);
    Task<NodeGroupDto> GetByIdAsync(Guid id);
    Task<List<NodeGroupDto>> GetAllAsync();
}