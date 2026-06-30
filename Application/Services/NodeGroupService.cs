using Application.Abstractions;
using Domain.DTOs.NodeGroups;
using Domain.Models;
using MapsterMapper;

namespace Application.Services;

public class NodeGroupService(INodeGroupRepository nodeGroupRepository, 
    IMapper mapper) : INodeGroupService
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

    public async Task<NodeGroupDto> UpdateAsync(Guid id, UpdateNodeGroupDto dto)
    {
        var group = await nodeGroupRepository.EnsureExistsAsync(id);
        
        if (dto.Name != null)
            group.Name = dto.Name;
        if (dto.Description != null)
            group.Description = dto.Description;
        
        nodeGroupRepository.Update(group);
        await nodeGroupRepository.SaveChangesAsync();
        
        return mapper.Map<NodeGroupDto>(group);
    }

    public async Task DeleteAsync(Guid id)
    {
        var group = await nodeGroupRepository.EnsureExistsAsync(id);
        nodeGroupRepository.Delete(group);
        await nodeGroupRepository.SaveChangesAsync();
    }

    public async Task<List<NodeGroupDto>> GetAllAsync()
    {
        var groups = await nodeGroupRepository.GetAllAsync();
        return mapper.Map<List<NodeGroupDto>>(groups);
    }

    public async Task<NodeGroupDto> GetByIdAsync(Guid id)
    {
        var group = await nodeGroupRepository.EnsureExistsAsync(id);
        return mapper.Map<NodeGroupDto>(group);
    }
}