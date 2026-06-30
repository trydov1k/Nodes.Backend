using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.NodeGroups;

public class CreateNodeGroupDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;
}