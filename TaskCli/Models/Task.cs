
namespace TaskCli.Models;

public class Task
{
    public string? AssignmentInfo { get; }
    public DateOnly? Completed { get; set; }
    public bool? Deleted { get; set; }
    public DateOnly? Due { get; set; }
    public string ETag { get; } = null!;
    public bool? Hidden { get; }
    public string Id { get; } = null!;
    public Task? Parent { get; set; }
    public string Kind { get; } = "task#task";
    public string Notes { get; set; } = string.Empty;
    public int Position { get; }
    public string SelfLink { get; } = null!;


}