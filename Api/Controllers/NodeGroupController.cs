using Application.Services;
using Domain.DTOs;
using Domain.DTOs.NodeGroups;
using Domain.DTOs.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NodeGroupController(INodeGroupService nodeGroupService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NodeGroupDto>>> Create([FromBody] CreateNodeGroupDto dto)
    {
        var createdGroup = await nodeGroupService.CreateAsync(dto);
        return CreatedAtAction(
            nameof(GetById),
            new { id = createdGroup.GroupId },
            ApiResponse.Success(createdGroup));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<NodeGroupDto>>>> GetAll([FromQuery] PaginationQuery query)
    {
        var result = await nodeGroupService.GetPagedAsync(query);
        return Ok(ApiResponse.Success(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<NodeGroupDto>>> GetById([FromRoute] Guid id)
    {
        var group = await nodeGroupService.GetByIdAsync(id);
        return Ok(ApiResponse.Success(group));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<NodeGroupDto>>> Update([FromRoute] Guid id,
        [FromBody] UpdateNodeGroupDto dto)
    {
        var updatedGroup = await nodeGroupService.UpdateAsync(id, dto);
        return Ok(ApiResponse.Success(updatedGroup));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await nodeGroupService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}/nodes")]
    public async Task<ActionResult<ApiResponse<PagedResult<NodeDto>>>> GetGroupNodes([FromRoute] Guid id, [FromQuery] PaginationQuery query)
    {
        var result = await nodeGroupService.GetNodesByGroupId(id, query);
        return Ok(ApiResponse.Success(result));
    }
}