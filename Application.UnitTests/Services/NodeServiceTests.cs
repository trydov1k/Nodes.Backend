using Application.Abstractions;
using Application.Services;
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