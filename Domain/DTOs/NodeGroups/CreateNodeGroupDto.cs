using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.NodeGroups;

public class CreateNodeGroupDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
}