using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using ToDoApp.Extensions;
using ToDoApp.Models;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly ITaskApiService _taskApiService;
    private readonly IAuthService _authService;

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

    public MainPageViewModel(ITaskApiService taskApiService, IAuthService authService)
    {
        _taskApiService = taskApiService;
        _authService = authService;
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
        var userId = await _authService.GetUserIdOrRedirectAsync();

        if (userId is null) return;

        var tasks = await _taskApiService.GetTasksAsync(userId.Value);

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
            var userId = await _authService.GetUserIdOrRedirectAsync();

            if (userId is null) return;

            var newTaskItem = new TaskItem { UserId = userId.Value, TaskDescription = NewTaskDescription };

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
