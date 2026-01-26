/*
    Is this class needed or should i just use Dictonary
*/

namespace TaskCli.Model;

public class ToDoList
{
    public required string Title { get; set; }
    public List<ToDoItem> Items { get; set; } = [];
}