using Domain.DTOs.Nodes;

namespace Application.Services;

public interface INodeService
{
    Task<NodeDto> CreateAsync(CreateNodeDto dto);
    Task<NodeDto> UpdateAsync(Guid id, UpdateNodeDto dto);
    Task DeleteAsync(Guid id);
    Task<NodeDto> GetByIdAsync(Guid id);
    Task<List<NodeDto>> GetAllAsync();
    
    Task<NodeDto> DetachFromGroupAsync(Guid id);
}