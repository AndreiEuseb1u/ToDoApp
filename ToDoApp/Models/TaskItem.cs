using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoApp.Models;

public partial class TaskItem : ObservableObject
{
    [ObservableProperty]
    private string _taskDescription;

    [ObservableProperty]
    private bool _state;

    public TaskItem(string description, bool state)
    {
        TaskDescription = description;
        State = state;
    }
}
