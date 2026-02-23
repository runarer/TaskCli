using Spectre.Console;
using Spectre.Console.Rendering;
using TaskCli.Controller;
using TaskCli.Model;

namespace TaskCli.Views;

public class ConsoleView
{

    public string[] MenuItems { get; } = ["Yesterday", "Today", "Tommorrow", "Select List", "Add List", "Rename List", "Delete List"];
    public Layout Layout { get; private set; } = new Layout();
    private ConsoleViewOptions _options;

    public ConsoleView(ConsoleViewOptions? options = null)
    {
        _options = (options is null) ? new ConsoleViewOptions() : options;

        Layout = new Layout("Root")
            .SplitColumns(
                new Layout("Menu").Size(16),
                new Layout("ToDoList").Ratio(1),
                new Layout("Info").Ratio(1));

        Layout["Menu"].Update(CreateMenu(1));
        Layout["ToDoList"].Update(new Panel("Select List to Start").BorderColor(Color.Green).Expand());
        Layout["Info"].Update(new Panel("Task Information").BorderColor(Color.Yellow).Expand());

    }

    public void Render(Panels selectedPanel, int menuIndex, string listName, List<RenderListItem>? renderList, int listIndex)
    {
        Layout["Menu"].Update(CreateMenu(selectedPanel == Panels.Menu ? menuIndex : -1));

        Layout["ToDoList"].Update(CreateToDoListPanel(listName, renderList, listIndex).BorderColor(Color.Green).Expand());

        Layout["Info"].Update(CreateNotePanel(renderList?[listIndex].Item).BorderColor(Color.Yellow).Expand());
    }

    private Panel CreateNotePanel(ToDoItem? item)
    {
        return new Panel(new Markup("From the new method"));
    }

    private Panel CreateToDoListPanel(string listName, List<RenderListItem>? renderList, int listIndex)
    {
        if (renderList is null)
            return new Panel("Select a list to open");

        int viewWindowHeight = AnsiConsole.Console.Profile.Height - 2;
        int start = Math.Max(0, listIndex - viewWindowHeight + 2);
        int end = start + viewWindowHeight - 1;

        List<IRenderable> tree = [];

        int index = 0;
        foreach (var item in renderList)
        {
            if (index >= start && index < end)
                tree.Add(new Padder(
                        CreateToDoItemLine(item.Item, listIndex == index),
                        new Padding(2 * item.Indent, 0)
                ));
            index++;
        }

        if (viewWindowHeight < renderList.Count && listIndex < renderList.Count - 1)
            return new Panel(new Rows(new Rows(tree), new Markup("..."))).Header(listName);

        return new Panel(new Rows(tree)).Header(listName);
    }

    private Rows CreateMenu(int selected)
    {
        string selectedColor = "blue";
        string unselectedColor = "green";
        string infoColor = "gray";
        string infoColorKey = "white";

        var item = new Rows(
            new Rule().Border(BoxBorder.None),
            new Markup($"[{((selected == 0) ? selectedColor : unselectedColor)}]{MenuItems[0]}[/]"),
            new Markup($"[{((selected == 1) ? selectedColor : unselectedColor)}]{MenuItems[1]}[/]"),
            new Markup($"[{((selected == 2) ? selectedColor : unselectedColor)}]{MenuItems[2]}[/]"),
            new Rule().Border(BoxBorder.None),
            new Markup($"[{((selected == 3) ? selectedColor : unselectedColor)}]{MenuItems[3]}[/]"),
            new Markup($"[{((selected == 4) ? selectedColor : unselectedColor)}]{MenuItems[4]}[/]"),
            new Markup($"[{((selected == 5) ? selectedColor : unselectedColor)}]{MenuItems[5]}[/]"),
            new Markup($"[{((selected == 6) ? selectedColor : unselectedColor)}]{MenuItems[6]}[/]"),
            new Rule().Border(BoxBorder.None),
            new Rule().Border(BoxBorder.None),
            new Markup($"[{infoColorKey}]<Space>[/][{infoColor}] Check[/]"),
            new Markup($"[{infoColorKey}]A[/]dd Task[{infoColor}][/]"),
            new Markup($"[{infoColorKey}]E[/]dit Task[{infoColor}][/]"),
            new Markup($"[{infoColorKey}]M[/]ove To List[{infoColor}][/]"),
            new Markup($"[{infoColorKey}]D[/]elete Task[{infoColor}][/]"),
            new Markup($"[{infoColorKey}]Q[/][{infoColor}]uit[/]")
        );

        return item;
    }

    private Panel CreateNotePanel()
    {

        return new Panel(new Markup("From the new method"));
    }

    private static Markup CreateToDoItemLine(ToDoItem item, bool selected)
    {
        return new Markup($"{(item.Completed ? ":check_mark_button:" : ":green_square:")} [{(selected ? "blue" : "yellow")}]{item.Title}[/]");
    }

    public static Actions GetMenuAction(int choice) => choice switch
    {
        0 => Actions.OpenListYesterday,
        1 => Actions.OpenListToday,
        2 => Actions.OpenListTomorrow,
        3 => Actions.SelectList,
        4 => Actions.AddList,
        5 => Actions.RenameList,
        6 => Actions.DeleteList,
        _ => throw new NotSupportedException("Unknokn menu selection")
    };

}

public class ConsoleViewOptions
{
    public string WarningColor { get; set; } = "red";
    public string ListColorSelected { get; set; } = "blue";
    public string ListColorUnselected { get; set; } = "green";
}
