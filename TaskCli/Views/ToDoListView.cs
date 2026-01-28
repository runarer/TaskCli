using System.Text;
using Spectre.Console;
using TaskCli.Model;

public class ToDoListView
{

    private ToDoList _todoList;
    private Table _table;

    public ToDoListView(ToDoList todoList)
    {
        _todoList = todoList;
        _table = new();
        _table.AddColumn($"[]{_todoList.Title}[/]");
        _table.Border(TableBorder.None);
        _table.AddEmptyRow();
    }

    public void Clear()
    {
        AnsiConsole.Clear();
    }

    public void Render(int selectedIndex)
    {
        ClearTable();
        if (_todoList.Items.Count < 1)
        {
            _table.AddRow(new Markup("[red]List is empty!"));
            return;
        }

        for (int i = 0; i < _todoList.Items.Count; i++)
        {





            // _table.AddRow(new Markup($"[{textColor}]{_todoList.Items[i]}[/]"));

            // if ()

        }
    }

    private void ClearTable()
    {
        while (_table.Rows.Count > 0)
            _table.RemoveRow(0);
    }

    private Markup CreateRow(ToDoItem item, bool selected)
    {
        string textColor = selected ? "blue" : "green";

        StringBuilder task = new();
        // Add Checkbox
        task.Append(item.Completed ? 'X' : 'O');

        // Name, and blue if selected
        task.Append(" [");
        task.Append(textColor);
        task.Append(']');
        task.Append(item.Title);
        task.Append("[/]");

        // Add due date
        if (item.Due.HasValue)
        {
            task.Append($"\t[pink]{item.Due.Value.ToShortDateString()}[/]");
        }

        if (!string.IsNullOrWhiteSpace(item.Notes))
        {
            task.Append($"\n{item.Notes}");
        }

        return new Markup(task.ToString());
    }


}