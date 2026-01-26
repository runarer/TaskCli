


namespace TaskCli;

public class TaskCli
{
    public async static void Main(string[] args)
    {
        IToDoListsStorage todoStorage = new Services.GoogleTaskToModel("client_secret.json");
    }
}



