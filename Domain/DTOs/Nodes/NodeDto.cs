using Domain.DTOs.NodeGroups;

namespace Domain.DTOs.Nodes;

public class NodeDto
{
    public Guid NodeId { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public NodeGroupDto? Group { get; set; }
}