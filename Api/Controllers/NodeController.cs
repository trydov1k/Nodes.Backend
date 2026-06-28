using Data.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NodeController(INodeService nodeService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create()
    { 
        return Ok();
    }
}