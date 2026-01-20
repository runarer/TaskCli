
namespace TaskCli.Models;

public abstract class Task
{
    public string Title { get; set; } = null!;
    public TaskStatus Status { get; set; }
    public DateOnly? CompletedOn { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateOnly? Due { get; set; }

    public int Position { get; set; }

    public DateOnly Updated { get; set; }
}

public sealed class MainTask : Task
{
    public List<SubTask> SubTasks { get; set; } = [];
}

public sealed class SubTask : Task
{

}


// public class Task
// {
//     public string? AssignmentInfo { get; }
//     public DateOnly? Completed { get; set; }
//     public bool? Deleted { get; set; }
//     public DateOnly? Due { get; set; }
//     public string ETag { get; } = null!;
//     public bool? Hidden { get; }
//     public string Id { get; } = null!;
//     public Task? Parent { get; set; }
//     public string Kind { get; } = "task#task";
//     public string Notes { get; set; } = string.Empty;
//     public int Position { get; }
//     public string SelfLink { get; } = null!;


// }