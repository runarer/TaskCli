using Spectre.Console;
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
        Layout["ToDoList"].Update(new Panel("2/4 width (half)").BorderColor(Color.Green).Expand());
        Layout["Info"].Update(new Panel("1/4 width").BorderColor(Color.Yellow).Expand());

    }

    public void Render(LiveDisplayContext ctx, Panels selectedPanel, int selectedItem)
    {

        Layout["Menu"].Update(CreateMenu(selectedPanel == Panels.Menu ? selectedItem : -1));
        Layout["ToDoList"].Update(new Panel("2/4 width (half)").BorderColor(Color.Green).Expand());
        Layout["Info"].Update(new Panel("1/4 width").BorderColor(Color.Yellow).Expand());
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

}

public class ConsoleViewOptions
{
    public string WarningColor { get; set; } = "red";
    public string ListColorSelected { get; set; } = "blue";
    public string ListColorUnselected { get; set; } = "green";
}
