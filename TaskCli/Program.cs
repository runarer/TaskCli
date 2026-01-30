


using Spectre.Console;
using Spectre.Console.Rendering;
using TaskCli.Controller;
using TaskCli.Model;
using TaskCli.Views;

namespace TaskCli;

public class TaskCli
{

    private static string[] _menuItems = ["Yesterday", "Today", "Tommorrow", "Select List", "Add List", "Rename List", "Delete List"];
    public static async Task Main(string[] args)
    {

        // var cancellationToken = new CancellationTokenSource();
        // Console.CancelKeyPress += (_, e) => { e.Cancel = true; cancellationToken.Cancel(); };

        // IToDoListsStorage todoStorage = new Services.GoogleTaskToModel("client_secret.json");
        // ToDoLists toDoLists = await ToDoLists.CreateAsync(todoStorage);

        // ConsoleView consoleView = new();

        // MainController controller = new(toDoLists, consoleView);

        // await controller.RunAsync(cancellationToken.Token);


        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Menu").Size(16),
                new Layout("ToDoList").Ratio(1),
                new Layout("Info").Ratio(1));

        layout["Menu"].Update(CreateMenu(1));
        layout["ToDoList"].Update(new Panel("2/4 width (half)").BorderColor(Color.Green).Expand());
        layout["Info"].Update(new Panel("1/4 width").BorderColor(Color.Yellow).Expand());

        // AnsiConsole.Write(layout);

        /* det kan hende at vi må gå ut av live view når select list eller add/edit er valgt.
            wrap alt i en while loop og gå inn og ut av live view etter behov.

            Q -> gå ut av begge loops.
        */

        bool runApp = true;
        int selectedItem = 1;
        Panels selectedPanel = Panels.Menu;
        Actions action = Actions.Quit;
        ToDoList? currentList = null;

        while (runApp)
        {
            AnsiConsole.Clear();
            AnsiConsole.Live(layout).Start(ctx =>
            {
                bool runLiveDisplay = true;


                while (runLiveDisplay)
                {
                    // Render
                    layout["Menu"].Update(CreateMenu(selectedPanel == Panels.Menu ? selectedItem : -1));
                    layout["ToDoList"].Update(new Panel("2/4 width (half)").BorderColor(Color.Green).Expand());
                    layout["Info"].Update(new Panel("1/4 width").BorderColor(Color.Yellow).Expand());
                    ctx.Refresh();

                    // Wait for input
                    var keyPress = Console.ReadKey(intercept: true);

                    // Handle input
                    switch (keyPress.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (selectedPanel == Panels.Menu)
                                selectedItem = Math.Max(0, selectedItem - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            if (selectedPanel == Panels.Menu)
                                selectedItem = Math.Min(6, selectedItem + 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            if (selectedPanel == Panels.ToDoList)
                                selectedPanel = Panels.Menu;
                            break;
                        case ConsoleKey.RightArrow:
                            if (selectedPanel == Panels.Menu)
                                selectedPanel = Panels.ToDoList;
                            break;
                        case ConsoleKey.Enter: //If menu then select action, if ToDoList then expand/collapse subtasks
                            if (selectedPanel == Panels.Menu)
                            {
                                action = GetMenuAction(_menuItems[selectedItem]);
                                runLiveDisplay = false;
                                break;
                            }
                            if (selectedPanel == Panels.ToDoList)
                            {

                            }
                            break;
                        case ConsoleKey.Spacebar: //Toggle completion for task
                            break;
                        case ConsoleKey.A: // Add a task
                            action = Actions.AddTask;
                            runLiveDisplay = false;
                            break;
                        case ConsoleKey.D: // Delete a task
                            action = Actions.DeleteTask;
                            break;
                        case ConsoleKey.E: // Edit a task
                            action = Actions.EditTask;
                            runLiveDisplay = false;
                            break;
                        case ConsoleKey.I: //Indent/unindent a task
                            break;
                        case ConsoleKey.M: // Move a task -> select list to move to
                            action = Actions.MoveTask;
                            runLiveDisplay = false;
                            break;
                        case ConsoleKey.Q:
                            action = Actions.Quit;
                            runLiveDisplay = false;
                            break;
                    }
                }
            });
            switch (action)
            {
                case Actions.Quit:
                    runApp = false;
                    break;
                case Actions.SelectList:
                    currentList = SelectList(); // TODO: null handling
                    break;
                case Actions.AddList:
                    ToDoList newList = CreateList();
                    currentList = newList; // TODO: null handling
                    break;
                case Actions.RenameList:
                    ToDoList listToRename = SelectList();
                    RenameList(listToRename); // TODO: null handling
                    break;
                case Actions.DeleteList:
                    ToDoList listToDelete = SelectList();
                    DeleteList(listToDelete); // TODO: null handling
                    break;
                case Actions.AddTask:
                    ToDoItem item = AddTask();
                    //TODO: null handling
                    //TODO: Add to current List
                    break;
                case Actions.EditTask:
                    // EditTask();
                    break;
                case Actions.DeleteTask:
                    // DeleteTask();
                    break;
                case Actions.MoveTask:
                    ToDoList toList = SelectList();
                    // MoveTask();
                    break;
                default:
                    throw new NotSupportedException("An unknown action occured");
            }
        }
        AnsiConsole.Clear();
    }
    private static Rows CreateMenu(int selected)
    {
        string selectedColor = "blue";
        string unselectedColor = "green";
        string infoColor = "gray";
        string infoColorKey = "white";

        var item = new Rows(
            new Rule().Border(BoxBorder.None),
            new Markup($"[{((selected == 0) ? selectedColor : unselectedColor)}]{_menuItems[0]}[/]"),
            new Markup($"[{((selected == 1) ? selectedColor : unselectedColor)}]{_menuItems[1]}[/]"),
            new Markup($"[{((selected == 2) ? selectedColor : unselectedColor)}]{_menuItems[2]}[/]"),
            new Rule().Border(BoxBorder.None),
            new Markup($"[{((selected == 3) ? selectedColor : unselectedColor)}]{_menuItems[3]}[/]"),
            new Markup($"[{((selected == 4) ? selectedColor : unselectedColor)}]{_menuItems[4]}[/]"),
            new Markup($"[{((selected == 5) ? selectedColor : unselectedColor)}]{_menuItems[5]}[/]"),
            new Markup($"[{((selected == 6) ? selectedColor : unselectedColor)}]{_menuItems[6]}[/]"),
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
    private static Actions GetMenuAction(string choice) => choice switch
    {
        "Select List" => Actions.SelectList,
        "Add List" => Actions.AddList,
        "Rename List" => Actions.RenameList,
        "Delete List" => Actions.DeleteList,
        _ => throw new NotSupportedException("Unknokn menu selection")
    };




    private static ToDoList SelectList()
    {
        throw new NotImplementedException("SelectedList not implemented yet");
    }

    private static ToDoList CreateList()
    {
        throw new NotImplementedException("CreateList not implemented yet");
    }

    private static bool RenameList(ToDoList list)
    {
        throw new NotImplementedException("RenameList not implemented yet");
    }
    private static bool DeleteList(ToDoList list)
    {
        throw new NotImplementedException("DeleteList not implemented yet");
    }

    private static ToDoItem AddTask()
    {
        throw new NotImplementedException("AddTask not implemented yet");
    }
    private static bool EditTask(ToDoItem item)
    {
        throw new NotImplementedException("EditTask not implemented yet");
    }

    private static bool DeleteTask(ToDoItem item)
    {
        throw new NotImplementedException("DeleteTask not implemented yet");
    }

}

public enum Panels
{
    Menu, ToDoList, Information
}

public enum Actions
{
    Quit, AddTask, EditTask, MoveTask, DeleteTask, SelectList, AddList, RenameList, DeleteList,
}