using System.Text;
using Spectre.Console;
using TaskCli.Model;
using TaskCli.Views;
using TaskCli.Services;

namespace TaskCli.Controller;

public class MainController
{
    // Need to take model and view objects.
    private ToDoList? _currentList = null;
    private ConsoleView _consoleView;
    private ToDoListService _model;

    private string[] _lists = [];

    public MainController(ToDoListService model, ConsoleView consoleView)
    {
        _model = model;
        _consoleView = consoleView;


    }

    // NOTE: Can add a index for list item so moving back to menu do not change position in list.
    public async Task RunAsync(CancellationToken token)
    {
        bool runApp = true;
        int selectedItem = 1;
        Panels selectedPanel = Panels.Menu;
        Actions action = Actions.Quit;

        _lists = await _model.GetListNames();

        while (runApp)
        {
            AnsiConsole.Clear();
            AnsiConsole.Live(_consoleView.Layout).Start(ctx =>
            {
                bool runLiveDisplay = true;


                while (runLiveDisplay)
                {
                    int numberOfTodoItemsInList = CountListItems(_currentList);

                    // Render
                    _consoleView.Render(ctx, _currentList, selectedPanel, selectedItem, numberOfTodoItemsInList);
                    ctx.Refresh();

                    // Wait for input
                    var keyPress = Console.ReadKey(intercept: true);

                    // Handle input
                    switch (keyPress.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (selectedPanel == Panels.Menu)
                                selectedItem = Math.Max(0, selectedItem - 1);
                            else if (selectedPanel == Panels.ToDoList)
                            {
                                selectedItem = Math.Max(0, selectedItem - 1);
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (selectedPanel == Panels.Menu)
                                selectedItem = Math.Min(_consoleView.MenuItems.Length - 1, selectedItem + 1);
                            else if (selectedPanel == Panels.ToDoList)
                            {
                                selectedItem = Math.Min(numberOfTodoItemsInList - 1, selectedItem + 1);
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (selectedPanel == Panels.ToDoList)
                            {
                                selectedPanel = Panels.Menu;
                                selectedItem = Math.Min(selectedItem, _consoleView.MenuItems.Length - 1);
                            }

                            break;
                        case ConsoleKey.RightArrow:
                            if (numberOfTodoItemsInList > 0 && selectedPanel == Panels.Menu)
                            {
                                selectedPanel = Panels.ToDoList;
                                selectedItem = Math.Min(selectedItem, numberOfTodoItemsInList - 1);
                            }
                            break;
                        case ConsoleKey.Enter: //If menu then select action, if ToDoList then expand/collapse subtasks
                            //Here i can change to a Dictionary<string,func>, or KeyValue<string,func>[], or I need to do
                            // a lookup into layout->menuitems
                            if (selectedPanel == Panels.Menu)
                            {
                                action = ConsoleView.GetMenuAction(selectedItem);
                                runLiveDisplay = false;
                                break;
                            }
                            if (selectedPanel == Panels.ToDoList)
                            {

                            }
                            break;
                        case ConsoleKey.Spacebar: //Toggle completion for task
                            if (selectedPanel == Panels.ToDoList)
                            {

                            }
                            break;
                        case ConsoleKey.A: // Add a task
                            action = Actions.AddTask;
                            runLiveDisplay = false;
                            break;
                        case ConsoleKey.D: // Delete a task
                            if (selectedPanel == Panels.ToDoList)
                            {
                                action = Actions.DeleteTask;
                            }
                            break;
                        case ConsoleKey.E: // Edit a task
                            if (selectedPanel == Panels.ToDoList)
                            {
                                action = Actions.EditTask;
                                runLiveDisplay = false;
                            }
                            break;
                        case ConsoleKey.I: //Indent/unindent a task
                            break;
                        case ConsoleKey.M: // Move a task -> select list to move to
                            if (selectedPanel == Panels.ToDoList)
                            {
                                action = Actions.MoveTask;
                                runLiveDisplay = false;
                            }
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
                    _currentList = await SelectList(); // TODO: null handling
                    break;
                case Actions.AddList:
                    ToDoList newList = await CreateList();
                    _currentList = newList; // TODO: null handling
                    break;
                case Actions.RenameList:
                    ToDoList listToRename = await SelectList();
                    await RenameList(listToRename); // TODO: null handling
                    break;
                case Actions.DeleteList:
                    ToDoList listToDelete = await SelectList();
                    await DeleteList(listToDelete); // TODO: null handling
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
                case Actions.OpenListYesterday:
                    break;
                case Actions.OpenListToday:
                    break;
                case Actions.OpenListTomorrow:
                    break;
                default:
                    throw new NotSupportedException("An unknown action occured");
            }
        }
    }

    private int CountToDoItems()
    {
        if (_currentList is null)
            return 0;

        int items = 0;

        foreach (var item in _currentList.Items)
        {
            foreach (var child in item.SubItems)
                items++;
            items++;
        }
        return items;
    }




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
        _lists = await _model.GetListNames();
        var selectedList = await AnsiConsole.PromptAsync(CreateSelectListPrompt(_lists, "Select When this should be done"));
        return await _model.GetList(selectedList);
    }

    private async Task<ToDoList> CreateList()
    {
        var cts = new CancellationTokenSource();
        string listName = await new TextPrompt<string>("[yellow]List name: [/]")
                                .ShowAsync(AnsiConsole.Console, cts.Token);

        await _model.AddList(listName);

        return await _model.GetList(listName);
    }

    private async Task RenameList(ToDoList list)
    {
        var cts = new CancellationTokenSource();
        string newName = await new TextPrompt<string>("[yellow]List name: [/]")
                                .DefaultValue(list.Title)
                                .ShowAsync(AnsiConsole.Console, cts.Token);
        if (newName != list.Title)
            await _model.UpdateListName(list.Title, newName);
    }
    private async Task<bool> DeleteList(ToDoList list)
    {

        if (await AnsiConsole.PromptAsync(
            new ConfirmationPrompt($"You sure you want to delete [red]{list.Title}[/]")
            {
                DefaultValue = false,
                ShowChoices = true,
                InvalidChoiceMessage = "[red]Invalid input. Please enter 'y' or 'n'.[/]",
            })
        )
            await _model.DeleteList(list.Title);
        return true;
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

    private static int CountListItems(ToDoList? list)
    {
        if (list is null)
            return 0;

        return list.Items.Sum(item => 1 + item.SubItems.Count);
    }
}

public enum Panels
{
    Menu, ToDoList, Information
}

public enum Actions
{
    Quit, AddTask, EditTask, MoveTask, DeleteTask, SelectList, AddList, RenameList, DeleteList, OpenListYesterday, OpenListToday, OpenListTomorrow
}