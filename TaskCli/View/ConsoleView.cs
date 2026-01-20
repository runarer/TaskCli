using TaskCli.Models;
using Spectre.Console;

namespace TaskCli.View;

public sealed class ConsoleView
{
    public void Render(TaskList list, int selectedItem)
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(new Rule($"[blue]{list.Name}[/]") { Justification = Justify.Left });
        for (int i = 0; i < list.Tasks.Count; i++)
        {
            if (i == selectedItem)
                AnsiConsole.MarkupLine($"[blue]{list.Tasks[i].Title}[/]");
            else
                AnsiConsole.WriteLine($"{list.Tasks[i].Title}");
        }
    }
}