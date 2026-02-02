using TaskCli.Model;

namespace TaskCli;

public interface IToDoListsStorage
{
    public Task<string[]> GetLists();
    public Task<ToDoList> GetList(string listName);
    public Task AddList(ToDoList list);
    public Task DeleteList(string list);
    public Task UpdateList(string oldList, string newList);
}