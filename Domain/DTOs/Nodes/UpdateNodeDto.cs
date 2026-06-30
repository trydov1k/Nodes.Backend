using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Nodes;

public class UpdateNodeDto
{
    [MinLength(1)]
    [MaxLength(30)]
    public string? Header { get; set; } = null;
    [MinLength(1)]
    [MaxLength(400)]
    public string? Text { get; set; } = null;
    public Guid? GroupId { get; set; } = null;
}