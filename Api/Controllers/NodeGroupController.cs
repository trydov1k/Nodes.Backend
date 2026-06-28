using Data.Services;
using Domain.DTOs;
using Domain.DTOs.NodeGroups;
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
        return Ok(ApiResponse.Success(createdGroup));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<NodeGroupDto>>>> GetAll()
    {
        var groups = await nodeGroupService.GetAllAsync();
        return Ok(ApiResponse.Success(groups));
    }
}