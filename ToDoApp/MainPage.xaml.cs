namespace ToDoApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnAddTaskTapped(object Sender, TappedEventArgs args)
        {
            EntryBorder.IsVisible = true;
        }

        private void OnEntryAdded(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(OnEntryAdd.Text))
            {
                OnEntryAdd.Text = string.Empty;
            }
        }

    }
}
