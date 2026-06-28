using Domain.DTOs.NodeGroups;

namespace Data.Services;

public interface INodeGroupService
{
    Task<NodeGroupDto> CreateAsync(CreateNodeGroupDto dto);

    Task<NodeGroupDto> GetAllAsync();
}