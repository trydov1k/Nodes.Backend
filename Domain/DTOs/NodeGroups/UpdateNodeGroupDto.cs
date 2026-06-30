using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.NodeGroups;

public class UpdateNodeGroupDto
{
    [MinLength(1)]
    [MaxLength(30)]
    public string? Name { get; set; } = null;
    [MinLength(1)]
    [MaxLength(100)]
    public string? Description { get; set; } = null;
}