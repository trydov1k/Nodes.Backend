namespace Domain.Models;

/// <summary>
/// Класс, описывающий группу заметок
/// </summary>
public class NodeGroup
{
    /// <summary>
    /// Id группы
    /// </summary>
    public Guid GroupId { get; set; }
    /// <summary>
    /// Имя группы
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Описание группы
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Список заметок, входящих в группу
    /// </summary>
    public ICollection<Node> Nodes { get; set; } = [];
}