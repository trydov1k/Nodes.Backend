using Data.Repositories;
using Domain.DTOs.NodeGroups;
using Domain.Models;
using MapsterMapper;

namespace Data.Services;

public class NodeGroupService(INodeGroupRepository nodeGroupRepository, IMapper mapper) : INodeGroupService
{
    public async Task<NodeGroupDto> CreateAsync(CreateNodeGroupDto dto)
    {
        var newGroup = new NodeGroup()
        {
            GroupId = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Nodes = []
        };

        await nodeGroupRepository.AddAsync(newGroup);
        await nodeGroupRepository.SaveChangesAsync();
        
        return mapper.Map<NodeGroupDto>(newGroup);
    }

    public async Task<NodeGroupDto> GetAllAsync()
    {
        var groups = await nodeGroupRepository.GetAllAsync();
        return mapper.Map<NodeGroupDto>(groups);
    }

    public async Task<NodeGroupDto> GetByIdAsync(Guid id)
    {
        var group = await nodeGroupRepository.EnsureExistsAsync(id);
        return mapper.Map<NodeGroupDto>(group);
    }
}