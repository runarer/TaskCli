namespace TaskCli.Model;

public class ToDoLists
{
    private IToDoListsStorage _storage;

    private Dictionary<string, ToDoList?> _lists = [];

    private ToDoLists(IToDoListsStorage storage)
    {
        _storage = storage;


    }

    public List<string> GetListNames()
    {
        return [.. _lists.Keys];
    }

    /// <summary>
    /// Use this to create a new object of ToDoLists
    /// </summary>
    /// <param name="storage">A reference to a storage</param>
    /// <param name="defaultList">A list to be included on construction</param>
    /// <returns>An object of ToDoLists populated with lists, the lists are empty except for defaultList</returns>
    /// <exception cref="InvalidOperationException">defaultList was not found in lists</exception>
    public static async Task<ToDoLists> CreateAsync(IToDoListsStorage storage, string? defaultList = null)
    {
        var todoLists = new ToDoLists(storage);

        var storedLists = await storage.GetLists();

        foreach (var list in storedLists)
        {
            todoLists._lists[list] = null;
        }

        if (defaultList is not null)
        {
            if (!todoLists._lists.ContainsKey(defaultList))
                throw new InvalidOperationException($"Default list {defaultList} could not be found in storage!");

            todoLists._lists[defaultList] = await todoLists._storage.GetList(defaultList);
        }

        return todoLists;
    }

    public async Task AddList(string name)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveList(string name)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateListName(string newName)
    {
        throw new NotImplementedException();
    }
}