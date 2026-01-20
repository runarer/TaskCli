using TaskCli.Models;
using Spectre.Console;

namespace TaskCli.View;

public sealed class ConsoleView
{
    public void Render()
    {
        AnsiConsole.Clear();
    }

    public void RenderList(TaskList list, int selectedItem)
    {
        AnsiConsole.Write(new Rule($"[blue]{list.Name}[/]") { Justification = Justify.Left });
        for (int i = 0; i < list.Tasks.Count; i++)
        {
            if (i == selectedItem)
                AnsiConsole.MarkupLine($"[lightblue]{list.Tasks[i].Title}[/]");
            else
                AnsiConsole.WriteLine($"{list.Tasks[i].Title}");
        }
    }
}