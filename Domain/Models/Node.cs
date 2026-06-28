namespace Domain.Models;

/// <summary>
/// Класс, который описывает заметку
/// </summary>
public class Node
{
    /// <summary>
    /// Id заметки
    /// </summary>
    public Guid NodeId { get; set; }
    /// <summary>
    /// Заголовок заметки (то, что видно в списке заметок)
    /// </summary>
    public string Header { get; set; } = string.Empty;
    /// <summary>
    /// Текст заметки
    /// </summary>
    public string Text { get; set; } =  string.Empty;
    
    /// <summary>
    /// Id группы, к которой относится заметка
    /// </summary>
    public Guid? GroupId { get; set; }
    /// <summary>
    /// Группа, к которой относится заметка
    /// </summary>
    public NodeGroup? Group { get; set; }
}