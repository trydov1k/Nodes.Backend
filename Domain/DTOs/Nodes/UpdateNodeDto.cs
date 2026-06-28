namespace Domain.DTOs.Nodes;

public class UpdateNodeDto
{
    public string? Header { get; set; } = null;
    public string? Text { get; set; } = null;
    public Guid? GroupId { get; set; } = null;
}