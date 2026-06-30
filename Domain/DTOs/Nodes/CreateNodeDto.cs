using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Nodes;

public class CreateNodeDto
{
    [Required]
    public string Header { get; set; } = string.Empty;
    [Required]
    public string Text { get; set; } = string.Empty;
    public Guid? GroupId { get; set; }
}