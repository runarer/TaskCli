using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;
using Spectre.Console;


using TaskCli.Models;
using TaskCli.Controller;
using TaskCli.View;


TaskCli.Models.TaskList tasklist = new()
{
    Name = "Testlist",
    Tasks = [new() { Title = "En task" }, new() { Title = "En annen task" }, new() { Title = "En task" }, new() { Title = "En annen task" }]
};


ConsoleView view = new();
TaskAppController controller = new(view, new Dictionary<string, TaskCli.Models.TaskList> { ["Testlist"] = tasklist }, "Testlist");

var token = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; token.Cancel(); };
await controller.RunAsync(token.Token);

// UserCredential credential;

// Create credentials and authorize
// TODO: If using file for secrets we should check if it exists and exit gracefully if not with and error message
// using var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read);
// credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
//     GoogleClientSecrets.FromStream(stream).Secrets,
//     [TasksService.Scope.Tasks],
//     "user",
//     CancellationToken.None,
//     new FileDataStore("Tasks.Auth.Store")).Result;
// Timeout?
// Cancel?
// Exceptions?

// Create service
// var taskService = new TasksService(new BaseClientService.Initializer()
// {
//     HttpClientInitializer = credential,
//     ApplicationName = "TaskCli",
// });

// Get Tasklists
// TaskLists results = taskService.Tasklists.List().Execute();

// foreach (TaskList list in results.Items)
// {
//     Console.WriteLine(list.Title);
//     var tasksRequest = taskService.Tasks.List(list.Id);
//     // Både ShowCompleted and ShowHidden må være true, Maxresult er valgfritt.
//     tasksRequest.ShowCompleted = true;
//     tasksRequest.ShowHidden = true;
//     tasksRequest.MaxResults = 100;

//     Tasks tasks = tasksRequest.Execute();
//     foreach (Google.Apis.Tasks.v1.Data.Task task in tasks.Items)
//     {
//         Console.WriteLine($"\t{(task.Parent is not null ? "\t" : "")}{((task.Completed is null) ? "O" : "X")} {task.Title}");

//     }
// }

// Add a Task
// Google.Apis.Tasks.v1.Data.Task newTask = taskService.Tasks.Insert(new Google.Apis.Tasks.v1.Data.Task { Title = "Added from CLI then moves to other list" }, results.Items[0].Id).Execute();
// taskService.Tasks.Move(results.Items[1].Id, newTask.Id).Execute();
