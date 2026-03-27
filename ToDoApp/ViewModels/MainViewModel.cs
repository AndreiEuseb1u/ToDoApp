using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using ToDoApp.Models;

namespace ToDoApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ObservableCollection<TaskItem> TaskList { get; } = new();

    [ObservableProperty]
    private string _newTaskDescription;

    [ObservableProperty]
    private bool _isEntryVisible;

    [ObservableProperty]
    private Color _doneButtonTextColor = Color.FromArgb("#8F8F8F");

    partial void OnNewTaskDescriptionChanged(string value)
    {
        DoneButtonTextColor = string.IsNullOrWhiteSpace(value) 
            ? Color.FromArgb("#8F8F8F") 
            : Color.FromArgb("#F2BB05");
    }

    [RelayCommand]
    private void ShowAddTask()
    {
        IsEntryVisible = true;
    }

    [RelayCommand]
    private void AddTask()
    {
        if (!string.IsNullOrWhiteSpace(NewTaskDescription))
        {
            TaskList.Add(new TaskItem(NewTaskDescription, false));
            NewTaskDescription = string.Empty;
        }
        IsEntryVisible = false;
    }

    [RelayCommand]
    private void DeleteTask(TaskItem task)
    {
        if (task != null)
        {
            TaskList.Remove(task);
        }
    }

    [RelayCommand]
    private void MarkTaskDone(TaskItem task)
    {
        if (task != null)
        {
            task.State = true;
        }
    }
}
