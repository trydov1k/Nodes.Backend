using Application.Abstractions;
using Domain.DTOs;
using Domain.DTOs.Nodes;
using Domain.Models;
using MapsterMapper;

namespace Application.Services;

public class NodeService(INodeRepository nodeRepository, 
    INodeGroupRepository nodeGroupRepository,
    IMapper mapper) : INodeService
{
    public async Task<NodeDto> CreateAsync(CreateNodeDto dto)
    {
        NodeGroup? group = null;
        if (dto.GroupId != null)
            group = await nodeGroupRepository.EnsureExistsAsync(dto.GroupId.Value);
        var newNode = new Node
        {
            NodeId = Guid.NewGuid(),
            Header = dto.Header,
            Text = dto.Text,
            GroupId = dto.GroupId,
            Group = group
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

    public async Task<NodeDto> GetByIdAsync(Guid id)
    {
        var node = await nodeRepository.EnsureExistsAsync(id);
        return mapper.Map<NodeDto>(node);
    }
    
    public async Task<PagedResult<NodeDto>> GetPagedAsync(PaginationQuery query)
    {
        var (items, totalCount) = await nodeRepository.GetPagedAsync(query.Page, query.PageSize);

        return new PagedResult<NodeDto>
        {
            Items = mapper.Map<List<NodeDto>>(items),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<NodeDto> DetachFromGroupAsync(Guid id)
    {
        var node = await nodeRepository.EnsureExistsAsync(id);
        node.GroupId = null;
        node.Group = null;
        nodeRepository.Update(node);
        await nodeRepository.SaveChangesAsync();
        return mapper.Map<NodeDto>(node);
    }
}