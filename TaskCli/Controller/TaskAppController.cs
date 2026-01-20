using Google.Apis.Tasks.v1.Data;
using Spectre.Console;
using TaskCli.Models;
using TaskCli.View;

namespace TaskCli.Controller;

public sealed class TaskAppController
{
    private ConsoleView _view;
    private Dictionary<string, Models.TaskList> _tasklists;
    private string _currentTaskList = string.Empty;
    private int _selectedItem = 0;

    public TaskAppController(ConsoleView view, Dictionary<string, Models.TaskList> taskLists, string startList)
    {
        _view = view;
        _tasklists = taskLists;
        _currentTaskList = startList;
    }

    public async System.Threading.Tasks.Task RunAsync(CancellationToken token)
    {


        while (!token.IsCancellationRequested)
        {
            _view.Render(_tasklists[_currentTaskList], _selectedItem);

            var key = Console.ReadKey(intercept: true);

            switch (key.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.K:
                case ConsoleKey.UpArrow:
                    _selectedItem = Math.Max(0, _selectedItem - 1);
                    break;
                case ConsoleKey.S:
                case ConsoleKey.L:
                case ConsoleKey.DownArrow:
                    _selectedItem = Math.Min(_tasklists[_currentTaskList].Tasks.Count - 1, _selectedItem + 1);
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Spacebar:
                    break;
                case ConsoleKey.Q:

                    break;
            }
        }
    }

}