using System.Collections.ObjectModel;
using System.Globalization;

namespace ToDoApp
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<TaskItem> _taskList = new();


        public MainPage()
        {
            InitializeComponent();
            TaskView.ItemsSource = _taskList;
        }

        public class TaskItem
        {
            public string TaskDescription { get; set; }
            public bool State { get; set; }

            public TaskItem (string description, bool state)
            {
                TaskDescription = description;
                State = state;
            }

        }

        private void OnAddTaskTapped(object Sender, TappedEventArgs args)
        {
            EntryBorder.IsVisible = true;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            DoneButton.TextColor = Color.FromArgb("#F2BB05");
        }

        private void OnEntryAdded(object sender, EventArgs e)
        {
            string taskDescription = OnEntryAdd.Text;
            if (!string.IsNullOrWhiteSpace(taskDescription))
            {
                _taskList.Add(new TaskItem(taskDescription, false));
                OnEntryAdd.Text = string.Empty;
            }
            EntryBorder.IsVisible = false;
        }

    }
}
