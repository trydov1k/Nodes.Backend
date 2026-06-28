using Domain;
using Domain.DTOs.NodeGroups;
using Domain.DTOs.Nodes;
using Domain.Models;
using Mapster;

namespace Api;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Node, NodeDto>.NewConfig()
            .Map(dest => dest.NodeId, src => src.NodeId)
            .Map(dest => dest.Header, src => src.Header)
            .Map(dest => dest.Text, src => src.Text)
            .Map(dest => dest.Group, src => src.Group);

        TypeAdapterConfig<NodeGroup, NodeGroupDto>.NewConfig()
            .Map(dest => dest.GroupId, src => src.GroupId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);
    }
}