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
        // ClearTable();
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
        while (_table.Rows.Count > 0)
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

/*

        var table = new Table();
        table.AddColumn("");
        table.Border(TableBorder.None);
        table.HideHeaders();

        string unselectedColor = "green";
        string selectedColor = "blue";

        AnsiConsole.Live(table).Start(ctx =>
        {
            bool finished = false;

            while (!finished)
            {
                while (table.Rows.Count > 0)
                    table.RemoveRow(0);
                for (int i = 0; i < rowContent.Count; i++)
                {
                    table.AddRow(new Markup($"[{((i == index) ? selectedColor : unselectedColor)}]{rowContent[i]}[/]"));
                }
                if (rowContent.Count < 1)
                {
                    table.AddRow("[Red]No content in list![/]");
                    index = 0;
                }

                ctx.Refresh();
                var keyPressed = Console.ReadKey(intercept: true);

                switch (keyPressed.Key)
                {
                    case ConsoleKey.Q:
                        finished = true;
                        break;
                    case ConsoleKey.UpArrow:
                        index = Math.Max(0, index - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        index = Math.Min(rowContent.Count - 1, index + 1);
                        break;
                    case ConsoleKey.D:
                        if (rowContent.Count < 1 || index < 0 || index >= rowContent.Count)
                            break;
                        index = Math.Min(rowContent.Count - 1, index + 1);
                        rowContent.RemoveAt(index);
                        break;
                    case ConsoleKey.A:
                        rowContent.Insert(index + 1, "Some new item");
                        index++;
                        break;
                }
            }


        });

*/