using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;


UserCredential credential;

// Create credentials and authorize
// TODO: If using file for secrets we should check if it exists and exit gracefully if not with and error message
using var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read);
credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
    GoogleClientSecrets.FromStream(stream).Secrets,
    [TasksService.Scope.Tasks],
    "user",
    CancellationToken.None,
    new FileDataStore("Tasks.Auth.Store")).Result;
// Timeout?
// Cancel?
// Exceptions?

// Create service
var taskService = new TasksService(new BaseClientService.Initializer()
{
    HttpClientInitializer = credential,
    ApplicationName = "TaskCli",
});

// Get Tasklists
TaskLists results = taskService.Tasklists.List().Execute();

foreach (TaskList list in results.Items)
{
    Console.WriteLine("\t\t" + list.Title);
}
