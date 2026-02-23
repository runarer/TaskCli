using TaskCli.Model;

namespace TaskCli.Views;

public record RenderListItem(ToDoItem Item, int Indent = 0, bool Bxpanded = false);