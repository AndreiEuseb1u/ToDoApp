using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoApp.Models;
public partial class TaskItem : ObservableObject
{
    [ObservableProperty]
    private Guid _userId;

    [ObservableProperty]
    private int _taskItemId;

    [ObservableProperty]
    private string _taskDescription = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CheckboxIconSource))]
    private bool _isCompleted;

    public string CheckboxIconSource => IsCompleted ? "solar_check_square_bold_24px.png" : "solar_check_square_linear_24px.png";
}
