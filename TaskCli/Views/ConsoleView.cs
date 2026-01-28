using Spectre.Console;
using TaskCli.Controller;
using TaskCli.Model;

namespace TaskCli.Views;

public class ConsoleView
{

    private Table _table = new Table();
    private ViewState _screenMode = ViewState.ToDoLists;
    private ConsoleViewOptions _options;

    public ConsoleView(ConsoleViewOptions? options = null)
    {
        _options = (options is null) ? new ConsoleViewOptions() : options;

        _table.AddColumn("Main"); // Name not shown
        _table.Border(TableBorder.None);
        _table.HideHeaders();
    }

    public void Render(LiveDisplayContext ctx, List<string> lists, int selectedList)
    {
        ClearTable();
        if (lists.Count == 0)
        {
            SetDefaultMessage();
            return;
        }

        for (int i = 0; i < lists.Count; i++)
        {
            _table.AddRow(new Markup($"[{((i == selectedList) ? _options.ListColorSelected : _options.ListColorUnselected)}]{lists[i]}[/]"));
        }
    }

    public Table GetMainTable() => _table;

    public void RemoveRow(int index)
    {
        if (index >= 0 && index < _table.Rows.Count)
        {
            _table.RemoveRow(index);
        }
    }

    private void SetDefaultMessage()
    {
        ClearTable();
        if (_screenMode == ViewState.ToDoLists)
        {
            _table.AddRow(new Markup($"[{_options.WarningColor}]No list selected[/]"));
        }
        else if (_screenMode == ViewState.ToDoList)
        {
            _table.AddRow(new Markup($"[{_options.WarningColor}]List is empty.[/]"));
        }
    }

    private void ClearTable()
    {
        for (int i = 0; i < _table.Rows.Count; i++)
        {
            _table.RemoveRow(0);
        }
    }

}

public class ConsoleViewOptions
{
    public string WarningColor { get; set; } = "red";
    public string ListColorSelected { get; set; } = "blue";
    public string ListColorUnselected { get; set; } = "green";
}