using System.Text;
using Spectre.Console;
using TaskCli.Model;
using TaskCli.Views;

namespace TaskCli.Controller;

public class MainController
{
    // Need to take model and view objects.
    private ToDoList? _currentList = null;
    private ViewState _currentState = ViewState.ToDoList;
    private ViewState _previousState = ViewState.ToDoList;

    private ConsoleView _consoleView;

    private ToDoLists _model;

    public MainController(ToDoLists model, ConsoleView consoleView)
    {
        _model = model;
        _consoleView = consoleView;
    }

    public async Task RunAsync(CancellationToken token)
    {
        bool run = true;

        AnsiConsole.Live(_consoleView.GetMainTable())
            // .AutoClear(true)
            .Start(ctx =>
        {

            while (run)
            {
                _consoleView.Render(ctx, ["Test", "Test2", "Test3"], 0);
                ctx.Refresh();

                var keyPress = Console.ReadKey(intercept: true);

                switch (keyPress.Key)
                {
                    case ConsoleKey.Q:
                        run = false;
                        break;
                    case ConsoleKey.H:
                        _currentState = ViewState.Help;
                        break;
                    case ConsoleKey.L:
                        _currentState = ViewState.ToDoLists;
                        break;
                    default:
                        //
                        break;
                }
            }
        });
    }

    public void BackToPreviousView()
    {
        _currentState = _previousState;
    }

    public void SetCurrentList(ToDoList list)
    {
        _currentList = list;
    }
}

public enum ViewState
{
    Help, ToDoList, ToDoLists
}