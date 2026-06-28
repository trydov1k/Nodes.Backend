using Data.Repositories;
using Domain;
using Domain.DTOs.Nodes;
using Domain.Models;
using MapsterMapper;

namespace Data.Services;

public class NodeService(INodeRepository nodeRepository, 
    INodeGroupRepository nodeGroupRepository,
    IMapper mapper) : INodeService
{
    public async Task<NodeDto> CreateAsync(CreateNodeDto dto)
    {
        var newNode = new Node
        {
            NodeId = Guid.NewGuid(),
            Header = dto.Header,
            Text = dto.Text,
            GroupId = dto.GroupId
        };
        
        await nodeRepository.AddAsync(newNode);
        await nodeRepository.SaveChangesAsync();
        
        return mapper.Map<NodeDto>(newNode);
    }

    public async Task<NodeDto> UpdateAsync(Guid id, UpdateNodeDto dto)
    {
        var oldNode = await nodeRepository.EnsureExistsAsync(id);
        
        if (dto.Header != null)
            oldNode.Header = dto.Header;
        if (dto.Text != null)
            oldNode.Text = dto.Text;
        if (dto.GroupId != null)
        {
            oldNode.GroupId = dto.GroupId;
            oldNode.Group = await nodeGroupRepository.EnsureExistsAsync(dto.GroupId.Value);
        }
            
        
        nodeRepository.Update(oldNode);
        await nodeRepository.SaveChangesAsync();
        return mapper.Map<NodeDto>(oldNode);
    }

    public async Task DeleteAsync(Guid id)
    {
        var node = await nodeRepository.EnsureExistsAsync(id);
        
        nodeRepository.Delete(node);
        await nodeRepository.SaveChangesAsync();
    }

    public async Task<NodeDto?> GetByIdAsync(Guid id)
    {
        var node = await nodeRepository.EnsureExistsAsync(id);
        return mapper.Map<NodeDto>(node);
    }

    public async Task<List<NodeDto>> GetAllAsync()
    {
        var nodes = await nodeRepository.GetAllAsync();
        
        return mapper.Map<List<NodeDto>>(nodes);
    }

    public async Task<List<NodeDto>> GetByGroupId(Guid groupId)
    {
        var nodes = await nodeRepository.GetByGroupId(groupId);
        return mapper.Map<List<NodeDto>>(nodes);
    }
}