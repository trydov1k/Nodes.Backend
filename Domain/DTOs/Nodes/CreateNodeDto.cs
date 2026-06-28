namespace Domain.DTOs.Nodes;

public class CreateNodeDto
{
    public string Header { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public Guid? GroupId { get; set; }
}