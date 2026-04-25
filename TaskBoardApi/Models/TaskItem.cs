using System.ComponentModel.DataAnnotations;
namespace TaskBoardApi.Models;
public class TaskItem {
    public int id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}