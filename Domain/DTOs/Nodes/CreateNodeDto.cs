using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Nodes;

public class CreateNodeDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(30)]
    public string Header { get; set; } = string.Empty;
    [Required]
    [MinLength(1)]
    [MaxLength(400)]
    public string Text { get; set; } = string.Empty;
    public Guid? GroupId { get; set; }
}