


using TaskCli.Model;

namespace TaskCli;

public class TaskCli
{
    public static async Task Main(string[] args)
    {
        IToDoListsStorage todoStorage = new Services.GoogleTaskToModel("client_secret.json");

        // Test if I get something back

        var lists = await todoStorage.GetLists();

        foreach (var l in lists)
            Console.WriteLine(l);

        var list = await todoStorage.GetList(lists[1]);
        foreach (var l in list.Items)
        {
            Console.WriteLine(l.Title);
            if (l.SubItems.Count > 0)
                foreach (var subItem in l.SubItems)
                    Console.WriteLine($"\t{subItem.Title}");
        }

    }
}



