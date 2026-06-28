namespace Domain.DTOs.NodeGroups;

public class NodeGroupDto
{
    public Guid GroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}