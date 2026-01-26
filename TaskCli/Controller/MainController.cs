using System.Text;
using TaskCli.Model;

namespace TaskCli.Controller;

public class MainController
{
    // Need to take model and view objects.
    private ToDoList? _currentList = null;
    private ViewState _currentState = ViewState.ToDoList;
    private ViewState _previousState = ViewState.ToDoList;

    private ToDoLists _model;

    public MainController(ToDoLists model)
    {
        _model = model;
    }

    public async Task RunAsync(CancellationToken token)
    {
        bool run = true;

        // Render something first

        while (run)
        {
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