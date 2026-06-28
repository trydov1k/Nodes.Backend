using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// Класс, который описывает заметку
/// </summary>
public class Node
{
    /// <summary>
    /// Id заметки
    /// </summary>
    public Guid NodeId { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Заголовок заметки (то, что видно в списке заметок)
    /// </summary>
    [MaxLength(10)]
    public string Header { get; set; } = string.Empty;
    /// <summary>
    /// Текст заметки
    /// </summary>
    public string Text { get; set; } =  string.Empty;

    /// <summary>
    /// Группа, к которой относится заметка
    /// </summary>
    public NodeGroup Group { get; set; } = new();
}