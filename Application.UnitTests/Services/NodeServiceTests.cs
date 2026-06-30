using Application.Abstractions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Nodes;
using Domain.Models;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Application.UnitTests.Services;

public class NodeServiceTests
{
    private readonly Mock<INodeRepository> _nodeRepo = new();
    private readonly Mock<INodeGroupRepository> _groupRepo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly NodeService _sut; // System Under Test — то, что тестируем
    
    public NodeServiceTests()
    {
        _sut = new NodeService(_nodeRepo.Object, _groupRepo.Object, _mapper.Object);
    }
    
    [Fact]
    public async Task CreateAsync_WithoutGroup_CreatesNode()
    {
        // Arrange
        var dto = new CreateNodeDto { Header = "Test", Text = "Body" };
        var expected = new NodeDto { Header = "Test", Text = "Body" };
        
        _mapper
            .Setup(m => m.Map<NodeDto>(It.IsAny<Node>()))
            .Returns(expected);
        
        // Act
        var result = await _sut.CreateAsync(dto);
        
        // Assert
        result.Header.Should().Be("Test");
        _nodeRepo.Verify(r => r.AddAsync(It.IsAny<Node>()), Times.Once);
        _nodeRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        _groupRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }
    [Fact]
    public async Task CreateAsync_WithGroup_AssignsGroup()
    {
        var groupId = Guid.NewGuid();
        var group = new NodeGroup { GroupId = groupId, Name = "G" };
        var dto = new CreateNodeDto { Header = "Test", Text = "Body", GroupId = groupId };

        _groupRepo.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);
        _mapper.Setup(m => m.Map<NodeDto>(It.IsAny<Node>())).Returns(new NodeDto());

        Node? captured = null;
        _nodeRepo
            .Setup(r => r.AddAsync(It.IsAny<Node>()))
            .Callback<Node>(n => captured = n)
            .Returns(Task.CompletedTask);

        await _sut.CreateAsync(dto);

        captured!.GroupId.Should().Be(groupId);
        captured.Group.Should().Be(group);
        _groupRepo.Verify(r => r.GetByIdAsync(groupId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithUnknownGroup_ThrowsKeyNotFound()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var dto = new CreateNodeDto { Header = "Test", Text = "Body", GroupId = groupId };
        _groupRepo
            .Setup(r => r.GetByIdAsync(groupId))
            .ReturnsAsync((NodeGroup?)null);
        // Act
        var act = () => _sut.CreateAsync(dto);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
    
    [Fact]
    public async Task UpdateAsync_UpdatesOnlyProvidedFields()
    {
        var nodeId = Guid.NewGuid();
        var node = new Node { NodeId = nodeId, Header = "Old", Text = "Text" };

        _nodeRepo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync(node);
        _mapper.Setup(m => m.Map<NodeDto>(node)).Returns(new NodeDto { NodeId = nodeId, Header = "New" });

        await _sut.UpdateAsync(nodeId, new UpdateNodeDto { Header = "New" });

        node.Header.Should().Be("New");
        node.Text.Should().Be("Text");
        _nodeRepo.Verify(r => r.Update(node), Times.Once);
        _nodeRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithGroup_AssignsGroup()
    {
        var nodeId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var group = new NodeGroup { GroupId = groupId, Name = "G" };
        var node = new Node { NodeId = nodeId, Header = "H", Text = "T" };

        _nodeRepo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync(node);
        _groupRepo.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);
        _mapper.Setup(m => m.Map<NodeDto>(node)).Returns(new NodeDto());

        await _sut.UpdateAsync(nodeId, new UpdateNodeDto { GroupId = groupId });

        node.GroupId.Should().Be(groupId);
        node.Group.Should().Be(group);
    }

    [Fact]
    public async Task UpdateAsync_NodeNotFound_ThrowsKeyNotFound()
    {
        var nodeId = Guid.NewGuid();
        _nodeRepo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync((Node?)null);

        var act = () => _sut.UpdateAsync(nodeId, new UpdateNodeDto { Header = "X" });

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_DeletesNode()
    {
        var nodeId = Guid.NewGuid();
        var node = new Node { NodeId = nodeId, Header = "H", Text = "T" };

        _nodeRepo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync(node);

        await _sut.DeleteAsync(nodeId);

        _nodeRepo.Verify(r => r.Delete(node), Times.Once);
        _nodeRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NodeNotFound_ThrowsKeyNotFound()
    {
        var nodeId = Guid.NewGuid();
        _nodeRepo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync((Node?)null);

        var act = () => _sut.DeleteAsync(nodeId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedDto()
    {
        var nodeId = Guid.NewGuid();
        var node = new Node { NodeId = nodeId, Header = "H", Text = "T" };
        var expected = new NodeDto { NodeId = nodeId, Header = "H" };

        _nodeRepo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync(node);
        _mapper.Setup(m => m.Map<NodeDto>(node)).Returns(expected);

        var result = await _sut.GetByIdAsync(nodeId);

        result.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ThrowsKeyNotFound()
    {
        var nodeId = Guid.NewGuid();
        _nodeRepo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync((Node?)null);

        var act = () => _sut.GetByIdAsync(nodeId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsPagedResult()
    {
        var nodes = new List<Node> { new() { NodeId = Guid.NewGuid(), Header = "H" } };
        var dtos = new List<NodeDto> { new() { Header = "H" } };
        var query = new PaginationQuery { Page = 1, PageSize = 20 };

        _nodeRepo.Setup(r => r.GetPagedAsync(1, 20)).ReturnsAsync((nodes, 1));
        _mapper.Setup(m => m.Map<List<NodeDto>>(nodes)).Returns(dtos);

        var result = await _sut.GetPagedAsync(query);

        result.Items.Should().BeSameAs(dtos);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(20);
        result.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task DetachFromGroupAsync_ClearsGroup()
    {
        // Arrange
        var nodeId = Guid.NewGuid();
        var node = new Node
        {
            NodeId = nodeId,
            Header = "H",
            Text = "T",
            GroupId = Guid.NewGuid()
        };

        _nodeRepo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync(node);
        _mapper.Setup(m => m.Map<NodeDto>(node)).Returns(new NodeDto { NodeId = nodeId });

        // Act
        await _sut.DetachFromGroupAsync(nodeId);

        // Assert
        node.GroupId.Should().BeNull();
        node.Group.Should().BeNull();
        _nodeRepo.Verify(r => r.Update(node), Times.Once);
        _nodeRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    
}