using TaskCli.Model;

namespace TaskCli.Views;

/// <summary>
/// A record for sending information needed for the rendering
/// </summary>
/// <param name="Item">The ToDoItem to render</param>
/// <param name="Indent">If and how many sublevels an item is</param>
public record RenderListItem(ToDoItem Item, int Indent = 0);