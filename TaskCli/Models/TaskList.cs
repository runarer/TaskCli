
namespace TaskCli.Models;

public sealed class TaskList
{
    public string Name { get; set; } = string.Empty;
    public List<MainTask> Tasks { get; set; } = [];
}