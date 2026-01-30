# TaskCli

A classic cli ToDo application, but it connects to Google Tasks.
!(Insert Image here!!!)[]

## TODO

### Init Project

- [x] Make a .NET solution, project and git setup
- [x] Install NuGet packages
  - [x] Google API
  - [x] Google API Auth
  - [x] Google API Task
- [x] Fix Key from Google Cloud

### Program

- [x] Connect to Auth
- [x] Get Tasklists
- [ ] Model Tasklist
- [ ] Model Task

### Endring

Kan erstattte enumen for actions med en dictionary<string,func>. en for menu action og en for keystrokes.
dictionary<string,func> for menu, KeyValue<string,func>[] kan være bedre fordi det beholder order.
Dictionary<ConsoleKey,func> for keystrokes, går dette?

RegisterMenuItem Title, function
RegisterShortKey Key, function, text

### ToDoList

Skal ToDoList ha en Count for totale antalle items?
Eller skal controller ha en private CountItems for listen, dette er raskt.
