using Application.Abstractions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.NodeGroups;
using Domain.DTOs.Nodes;
using Domain.Models;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Application.UnitTests.Services;

public class NodeGroupServiceTests
{
    private readonly Mock<INodeGroupRepository> _groupRepo = new();
    private readonly Mock<INodeRepository> _nodeRepo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly NodeGroupService _sut;

    public NodeGroupServiceTests()
    {
        _sut = new NodeGroupService(_groupRepo.Object, _nodeRepo.Object, _mapper.Object);
    }

    [Fact]
    public async Task CreateAsync_CreatesGroup()
    {
        var dto = new CreateNodeGroupDto { Name = "G", Description = "D" };
        var expected = new NodeGroupDto { Name = "G", Description = "D" };

        _mapper.Setup(m => m.Map<NodeGroupDto>(It.IsAny<NodeGroup>())).Returns(expected);

        NodeGroup? captured = null;
        _groupRepo
            .Setup(r => r.AddAsync(It.IsAny<NodeGroup>()))
            .Callback<NodeGroup>(g => captured = g)
            .Returns(Task.CompletedTask);

        var result = await _sut.CreateAsync(dto);

        result.Name.Should().Be("G");
        captured!.Name.Should().Be("G");
        captured.Description.Should().Be("D");
        captured.Nodes.Should().BeEmpty();
        _groupRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesOnlyProvidedFields()
    {
        var groupId = Guid.NewGuid();
        var group = new NodeGroup { GroupId = groupId, Name = "Old", Description = "Desc" };

        _groupRepo.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);
        _mapper.Setup(m => m.Map<NodeGroupDto>(group)).Returns(new NodeGroupDto { Name = "New" });

        await _sut.UpdateAsync(groupId, new UpdateNodeGroupDto { Name = "New" });

        group.Name.Should().Be("New");
        group.Description.Should().Be("Desc");
        _groupRepo.Verify(r => r.Update(group), Times.Once);
        _groupRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NotFound_ThrowsKeyNotFound()
    {
        var groupId = Guid.NewGuid();
        _groupRepo.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync((NodeGroup?)null);

        var act = () => _sut.UpdateAsync(groupId, new UpdateNodeGroupDto { Name = "X" });

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_DeletesGroup()
    {
        var groupId = Guid.NewGuid();
        var group = new NodeGroup { GroupId = groupId, Name = "G" };

        _groupRepo.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);

        await _sut.DeleteAsync(groupId);

        _groupRepo.Verify(r => r.Delete(group), Times.Once);
        _groupRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedDto()
    {
        var groupId = Guid.NewGuid();
        var group = new NodeGroup { GroupId = groupId, Name = "G" };
        var expected = new NodeGroupDto { GroupId = groupId, Name = "G" };

        _groupRepo.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);
        _mapper.Setup(m => m.Map<NodeGroupDto>(group)).Returns(expected);

        var result = await _sut.GetByIdAsync(groupId);

        result.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsPagedResult()
    {
        var groups = new List<NodeGroup> { new() { GroupId = Guid.NewGuid(), Name = "G" } };
        var dtos = new List<NodeGroupDto> { new() { Name = "G" } };
        var query = new PaginationQuery { Page = 2, PageSize = 10 };

        _groupRepo.Setup(r => r.GetPagedAsync(2, 10)).ReturnsAsync((groups, 15));
        _mapper.Setup(m => m.Map<List<NodeGroupDto>>(groups)).Returns(dtos);

        var result = await _sut.GetPagedAsync(query);

        result.Items.Should().BeSameAs(dtos);
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(15);
    }

    [Fact]
    public async Task GetNodesByGroupId_ReturnsPagedResult()
    {
        var groupId = Guid.NewGuid();
        var group = new NodeGroup { GroupId = groupId, Name = "G" };
        var nodes = new List<Node> { new() { NodeId = Guid.NewGuid(), Header = "H" } };
        var dtos = new List<NodeDto> { new() { Header = "H" } };
        var query = new PaginationQuery { Page = 1, PageSize = 20 };

        _groupRepo.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);
        _nodeRepo.Setup(r => r.GetByGroupId(groupId, 1, 20)).ReturnsAsync((nodes, 1));
        _mapper.Setup(m => m.Map<List<NodeDto>>(nodes)).Returns(dtos);

        var result = await _sut.GetNodesByGroupId(groupId, query);

        result.Items.Should().BeSameAs(dtos);
        result.TotalCount.Should().Be(1);
        _nodeRepo.Verify(r => r.GetByGroupId(groupId, 1, 20), Times.Once);
    }

    [Fact]
    public async Task GetNodesByGroupId_GroupNotFound_ThrowsKeyNotFound()
    {
        var groupId = Guid.NewGuid();
        _groupRepo.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync((NodeGroup?)null);

        var act = () => _sut.GetNodesByGroupId(groupId, new PaginationQuery());

        await act.Should().ThrowAsync<KeyNotFoundException>();
        _nodeRepo.Verify(r => r.GetByGroupId(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
}
