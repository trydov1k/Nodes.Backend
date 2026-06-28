using Data.Services;
using Domain.DTOs;
using Domain.DTOs.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NodeController(INodeService nodeService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NodeDto>>> Create([FromBody] CreateNodeDto dto)
    {
        var createdNode = await nodeService.CreateAsync(dto);
        return Ok(ApiResponse.Success(createdNode));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<NodeDto>>>> GetAll()
    {
        var nodes = await nodeService.GetAllAsync();
        return Ok(ApiResponse.Success(nodes));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<NodeDto>>> GetById([FromRoute] Guid id)
    {
        var node = await nodeService.GetByIdAsync(id);
        return Ok(ApiResponse.Success(node));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<NodeDto>>> Update([FromRoute] Guid id, [FromBody] UpdateNodeDto dto)
    {
        var updatedNode = await nodeService.UpdateAsync(id, dto);
        return Ok(ApiResponse.Success(updatedNode));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<NodeDto>>> Delete([FromRoute] Guid id)
    {
        await nodeService.DeleteAsync(id);
        return NoContent();
    }
}