using TaskCli.Model;

namespace TaskCli;

public interface IToDoListsStorage
{
    public Task<List<string>> GetLists();
    public Task<ToDoList> GetList(string listName);
    public Task AddList(ToDoList list);
    public Task RemoveList(ToDoList list);
    public Task UpdateList(ToDoList oldList, ToDoList newList);
}