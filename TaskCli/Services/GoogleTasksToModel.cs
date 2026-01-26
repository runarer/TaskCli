using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;

using TaskCli.Model;

namespace TaskCli.Services;

public class GoogleTaskToModel : IToDoListsStorage
{
    private TasksService _taskService;
    public GoogleTaskToModel(string secretsFileName)
    {
        using var stream = new FileStream(secretsFileName, FileMode.Open, FileAccess.Read);
        UserCredential _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        GoogleClientSecrets.FromStream(stream).Secrets,
        [TasksService.Scope.Tasks],
        "user",
        CancellationToken.None,
        new FileDataStore("Tasks.Auth.Store")).Result;

        _taskService = new TasksService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = _credential,
            ApplicationName = "TaskCli",
        });
    }

    public async Task<List<string>> GetLists()
    {
        TaskLists results = await _taskService.Tasklists.List().ExecuteAsync();

        return [.. results.Items.Select(item => item.Title)];
    }

    public async Task<ToDoList> GetList(string listName)
    {
        var lists = await _taskService.Tasklists.List().ExecuteAsync();

        var listId = (lists.Items.FirstOrDefault(list => list.Title == listName)?.Id)
            ?? throw new InvalidOperationException($" {listName} not found on Google tasks");

        var tasksRequest = _taskService.Tasks.List(listId);
        // Både ShowCompleted and ShowHidden må være true, Maxresult er valgfritt.
        tasksRequest.ShowCompleted = true;
        tasksRequest.ShowHidden = true;
        tasksRequest.MaxResults = 100;

        Tasks tasks = tasksRequest.Execute();

        // Get Main tasks, need to store Id and Parent so we can match them later.
        var tempItems = tasks.Items.Select(item => new
        {
            item.Id,
            item.Parent,
            Item = TaskToTodoItem(item)
        }).ToList(); // Need to make it a list so it's mutable.


        // Get Subtasks and add them to right parent
        foreach (var item in tempItems)
        {
            if (item.Parent is not null)
            {
                // Find the parent
                var parent = tempItems.First(p => p.Id == item.Parent);
                // Add item to parent
                parent.Item.SubItems.Add(item.Item);
            }
        }

        var todoList = new ToDoList()
        {
            Title = listName,
            Items = [.. tempItems.Where(item => item.Parent is null).Select(item => item.Item)]
        };

        return todoList;
    }

    private ToDoItem TaskToTodoItem(Google.Apis.Tasks.v1.Data.Task item)
    {
        return new ToDoItem()
        {
            Title = item.Title,
            UpdatedOn = DateTime.Parse(item.Updated),
            CreatedOn = DateTime.Parse(item.Updated),
            Completed = item.Completed is not null,
            Notes = item.Notes is null ? string.Empty : item.Notes,
            URLs = [],
            CompletedOn = item.Completed is null ? null : DateTime.Parse(item.Completed),
            Due = item.Due is null ? null : DateTime.Parse(item.Due),
        };
    }
}