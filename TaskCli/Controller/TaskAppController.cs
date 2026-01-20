using Spectre.Console;
using TaskCli.Models;
using TaskCli.View;

namespace TaskCli.Controller;

public sealed class TaskAppController
{
    private ConsoleView _view;

    public TaskAppController(ConsoleView view)
    {
        _view = view;
    }

    public async System.Threading.Tasks.Task RunAsync(CancellationToken token)
    {
        _view.Render();

        var key = Console.ReadKey(intercept: true);

        while (!token.IsCancellationRequested)
        {
            switch (key.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.K:
                case ConsoleKey.UpArrow:
                    break;
                case ConsoleKey.DownArrow:
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