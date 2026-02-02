namespace TaskCli.Model;

public class ToDoLists
{
    private IToDoListsStorage _storage;

    private ToDoLists(IToDoListsStorage storage)
    {
        _storage = storage;
    }

    public async Task<string[]> GetListNames()
    {
        return await _storage.GetLists();
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

        return todoLists;
    }

    public async Task<ToDoList> GetList(string name)
    {
        return await _storage.GetList(name);
    }

    public async Task AddList(string name)
    {
        var newList = new ToDoList { Title = name };

        await _storage.AddList(newList);
    }

    public async Task DeleteList(string name)
    {
        await _storage.DeleteList(name);
    }

    public async Task UpdateListName(string newName)
    {
        throw new NotImplementedException();
    }
}