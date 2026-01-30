using System.Text;
using Spectre.Console;
using TaskCli.Model;
using TaskCli.Views;

namespace TaskCli.Controller;

public class MainController
{
    // Need to take model and view objects.
    private ToDoList? _currentList = null;
    private ConsoleView _consoleView;
    private ToDoLists _model;

    private string[] _lists;

    public MainController(ToDoLists model, ConsoleView consoleView)
    {
        _model = model;
        _consoleView = consoleView;

        _lists = _model.GetListNames();
    }
    public async Task RunAsync(CancellationToken token)
    {
        bool runApp = true;
        int selectedItem = 1;
        Panels selectedPanel = Panels.Menu;
        Actions action = Actions.Quit;
        ToDoList? currentList = null;

        while (runApp)
        {
            AnsiConsole.Clear();
            AnsiConsole.Live(_consoleView.Layout).Start(ctx =>
            {
                bool runLiveDisplay = true;


                while (runLiveDisplay)
                {
                    // Render
                    _consoleView.Render(ctx, selectedPanel, selectedItem);
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
                                action = GetMenuAction(_consoleView.MenuItems[selectedItem]);
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

            AnsiConsole.Clear();

            switch (action)
            {
                case Actions.Quit:
                    runApp = false;
                    break;
                case Actions.SelectList:
                    currentList = await SelectList(); // TODO: null handling
                    break;
                case Actions.AddList:
                    ToDoList newList = CreateList();
                    currentList = newList; // TODO: null handling
                    break;
                case Actions.RenameList:
                    ToDoList listToRename = await SelectList();
                    RenameList(listToRename); // TODO: null handling
                    break;
                case Actions.DeleteList:
                    ToDoList listToDelete = await SelectList();
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
                    ToDoList toList = await SelectList();
                    // MoveTask();
                    break;
                default:
                    throw new NotSupportedException("An unknown action occured");
            }
        }
    }


    private Actions GetMenuAction(string choice) => choice switch
    {
        "Select List" => Actions.SelectList,
        "Add List" => Actions.AddList,
        "Rename List" => Actions.RenameList,
        "Delete List" => Actions.DeleteList,
        _ => throw new NotSupportedException("Unknokn menu selection")
    };


    //May not need to be nullable, can return string.Empty
    private SelectionPrompt<string> CreateSelectListPrompt(string[] lists, string title)
    {
        return new SelectionPrompt<string>()
            .Title(title)
            .PageSize(20)
            .AddChoices(lists);
    }

    private async Task<ToDoList> SelectList()
    {
        var selectedList = await AnsiConsole.PromptAsync(CreateSelectListPrompt(_lists, "Select When this should be done"));
        return await _model.GetList(selectedList);
    }

    private ToDoList CreateList()
    {
        throw new NotImplementedException("CreateList not implemented yet");
    }

    private bool RenameList(ToDoList list)
    {
        throw new NotImplementedException("RenameList not implemented yet");
    }
    private bool DeleteList(ToDoList list)
    {
        throw new NotImplementedException("DeleteList not implemented yet");
    }

    private ToDoItem AddTask()
    {
        // Prompt for title
        // Prompt for notes
        // Ask if duedate
        // If yes show date widget
        throw new NotImplementedException("AddTask not implemented yet");
    }
    private bool EditTask(ToDoItem item)
    {
        throw new NotImplementedException("EditTask not implemented yet");
    }

    private bool DeleteTask(ToDoItem item)
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