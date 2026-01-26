using TaskCli.Model;

namespace TaskCli;

public interface IToDoListsStorage
{
    public Task<List<string>> GetLists();
    public Task<ToDoList> GetList(string listName);
}