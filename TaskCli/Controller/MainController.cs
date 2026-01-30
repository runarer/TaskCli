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

    public MainController(ToDoLists model, ConsoleView consoleView)
    {
        _model = model;
        _consoleView = consoleView;
    }

    public async Task RunAsync(CancellationToken token)
    {

    }
}