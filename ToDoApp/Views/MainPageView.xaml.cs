using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class MainPageView : ContentPage
    {
        private readonly MainViewModel _viewModel;
        public MainPageView(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadTasksAsync();
        }
    }
}
