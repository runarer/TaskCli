


using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Rendering;
using TaskCli.Controller;
using TaskCli.Model;
using TaskCli.Views;
using TaskCli.Services;

namespace TaskCli;

public class TaskCli
{

    public static async Task Main(string[] args)
    {

        var cancellationToken = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cancellationToken.Cancel(); };

        IToDoListsStorage todoStorage = new Services.GoogleTaskToModel("client_secret.json");
        ToDoListService toDoLists = await ToDoListService.CreateAsync(todoStorage);

        ConsoleView consoleView = new();

        MainController controller = new(toDoLists, consoleView);

        await controller.RunAsync(cancellationToken.Token);



    }


}

