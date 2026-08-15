using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using ToDoApp.Models;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private static readonly Guid CurrentUserId = Guid.Parse("8fcb976d-51e1-4b95-9c77-d8949e0cd546"); // TODO: înlocuiește cu userId real, după autentificare
    private readonly ITaskApiService _taskApiService;

    public ObservableCollection<TaskItem> TaskList { get; } = new();

    [ObservableProperty]
    private string _newTaskDescription = string.Empty;

    [ObservableProperty]
    private bool _isEntryVisible;

    [ObservableProperty]
    private bool _isEdit;

    [ObservableProperty]
    private Color _doneButtonTextColor = Color.FromArgb("#8F8F8F");

    [ObservableProperty]
    private string _numberOfCompletedTasks = string.Empty;

    [ObservableProperty]
    private TaskItem _taskBeingEdited;

    public MainViewModel(ITaskApiService taskApiService)
    {
        _taskApiService = taskApiService;
    }

    partial void OnNewTaskDescriptionChanged(string value)
    {
        DoneButtonTextColor = string.IsNullOrWhiteSpace(value) 
            ? Color.FromArgb("#8F8F8F") 
            : Color.FromArgb("#F2BB05");
    }

    [RelayCommand]
    private void ShowAddOrModifyTask()
    {
        IsEntryVisible = true;
    }

    // === Data loading ===

    public async Task LoadTasksAsync()
    {
        var tasks = await _taskApiService.GetTasksAsync(CurrentUserId);

        TaskList.Clear();

        foreach (var task in tasks)
        {
            TaskList.Add(task);
        }
    }

    // === Commands ===

    [RelayCommand]
    private async Task AddOrModifyTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskDescription))
        {
            NewTaskDescription = string.Empty;

            return;
        }

        if (TaskBeingEdited is null)
        {
            var newTaskItem = new TaskItem { UserId = CurrentUserId, TaskDescription = NewTaskDescription };

            var createdTask = await _taskApiService.PostTaskAsync(newTaskItem);

            if (createdTask is null)
            {
                return;
            }

            TaskList.Add(createdTask);
        }
        else
        {
            TaskBeingEdited.TaskDescription = NewTaskDescription;

            await _taskApiService.PutTaskAsync(TaskBeingEdited.TaskItemId, TaskBeingEdited);

            TaskBeingEdited = null;
        }

        NewTaskDescription = string.Empty;

        IsEntryVisible = false;
    }

    [RelayCommand]
    private async Task DeleteTask(TaskItem taskItem)
    {
        if (taskItem is null)
        {
            return;
        }

        await _taskApiService.DeleteTaskAsync(taskItem.TaskItemId);

        TaskList.Remove(taskItem);
    }

    [RelayCommand]
    private async Task ModifyTaskState(TaskItem taskItem)
    {
        if (taskItem is null)
        {
            return;
        }

        taskItem.IsCompleted = !taskItem.IsCompleted;

        await _taskApiService.PutTaskAsync(taskItem.TaskItemId, taskItem);
    }

    [RelayCommand]
    private async Task ModifyTaskDescription(TaskItem taskItem)
    {
        if (taskItem is null)
        {
            return;
        }

        TaskBeingEdited = taskItem;
        NewTaskDescription = taskItem.TaskDescription;
        ShowAddOrModifyTask();
    }
}
