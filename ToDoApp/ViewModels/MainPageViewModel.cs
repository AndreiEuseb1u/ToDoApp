using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToDoApp.Common;
using ToDoApp.Dtos;
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
    private bool _isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

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

        await RefreshTasksAsync();
    }

    private async Task<bool> RefreshTasksAsync()
    {
        var result = await RunBusyAsync(() => _taskApiService.GetTasksAsync());

        if (result.Success is false)
        {
            return false;
        }

        TaskList.Clear();

        foreach (var task in result.Data!)
        {
            TaskList.Add(task);
        }

        return true;
    }

    private async Task HandleFailureAsync(ApiResult result)
    {
        if (result.ErrorType is not (ApiErrorType.Conflict or ApiErrorType.NotFound))
        {
            return;
        }

        var refreshed = await RefreshTasksAsync();

        if (refreshed is false || TaskBeingEdited is null)
        {
            return;
        }

        var refreshedTask = TaskList.FirstOrDefault(t => t.TaskItemId == TaskBeingEdited.TaskItemId);

        if (refreshedTask is not null)
        {
            TaskBeingEdited = refreshedTask;
            return;
        }

        TaskBeingEdited = null;
        NewTaskDescription = string.Empty;
        IsEntryVisible = false;
    }

    private async Task<TResult> RunBusyAsync<TResult>(Func<Task<TResult>> operation) where TResult : ApiResult
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            var result = await operation();

            if (result.ErrorType is ApiErrorType.NoResponse)
            {
                ErrorMessage = result.ErrorMessage ?? string.Empty;
            }

            return result;
        }
        finally
        {
            IsBusy = false;
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

        if (NewTaskDescription.Length > 1000)
        {
            return;
        }

        if (TaskBeingEdited is null)
        {
            var userId = await _authService.GetUserIdOrRedirectAsync();

            if (userId is null) 
            { 
                return; 
            }

            var request = new CreateTaskRequest(NewTaskDescription);

            var result = await RunBusyAsync(() => _taskApiService.PostTaskAsync(request));

            if (result.Success is false)
            {
                return;
            }

            TaskList.Add(result.Data!);
        }
        else
        {
            var request = new ModifyTaskRequest(
                NewTaskDescription,
                TaskBeingEdited.IsCompleted,
                TaskBeingEdited.RowVersion);

            var result = await RunBusyAsync(() => _taskApiService.PutTaskAsync(TaskBeingEdited.TaskItemId, request));

            if (result.Success is false)
            {
                await HandleFailureAsync(result);
                return;
            }

            TaskBeingEdited.TaskDescription = result.Data!.TaskDescription;
            TaskBeingEdited.RowVersion = result.Data.RowVersion;

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

        var result = await RunBusyAsync(() => _taskApiService.DeleteTaskAsync(taskItem.TaskItemId, taskItem.RowVersion));

        if (result.Success is false)
        {
            await HandleFailureAsync(result);
            return;
        }

        TaskList.Remove(taskItem);
    }

    [RelayCommand]
    private async Task ModifyTaskState(TaskItem taskItem)
    {
        if (taskItem is null)
        {
            return;
        }

        var request = new ModifyTaskRequest(
            taskItem.TaskDescription,
            !taskItem.IsCompleted,
            taskItem.RowVersion);

        var result = await RunBusyAsync(() => _taskApiService.PutTaskAsync(taskItem.TaskItemId, request));

        if (result.Success is false)
        {
            await HandleFailureAsync(result);
            return;
        }

        taskItem.IsCompleted = result.Data!.IsCompleted;
        taskItem.RowVersion = result.Data.RowVersion;

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
