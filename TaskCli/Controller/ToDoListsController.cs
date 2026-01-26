namespace TaskCli.Controller;

public class ToDoListsController(MainController main)
{


    public bool ReadKey(ConsoleKey key)
    {
        bool keyProcessed = false;

        switch (key)
        {
            case ConsoleKey.B:
                main.BackToPreviousView();
                keyProcessed = true;
                break;
        }

        return keyProcessed;
    }
}