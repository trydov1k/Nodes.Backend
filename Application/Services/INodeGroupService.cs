using Domain.DTOs;
using Domain.DTOs.NodeGroups;
using Domain.DTOs.Nodes;

namespace Application.Services;

public interface INodeGroupService
{
    Task<NodeGroupDto> CreateAsync(CreateNodeGroupDto dto);
    Task<NodeGroupDto> UpdateAsync(Guid id, UpdateNodeGroupDto dto);
    Task DeleteAsync(Guid id);
    Task<NodeGroupDto> GetByIdAsync(Guid id);
    Task<PagedResult<NodeGroupDto>> GetPagedAsync(PaginationQuery query);
    
    Task<PagedResult<NodeDto>> GetNodesByGroupId(Guid groupId, PaginationQuery query);
}