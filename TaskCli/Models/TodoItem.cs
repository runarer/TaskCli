
namespace TaskCli.Model;

public class ToDoItem
{
    public required string Title { get; set; }
    public bool Completed { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<Uri> URLs { get; set; } = [];
    public required DateTime CreatedOn { get; set; }
    public required DateTime UpdatedOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public DateTime? Due { get; set; }
    public List<ToDoItem> SubItems { get; set; } = [];
}