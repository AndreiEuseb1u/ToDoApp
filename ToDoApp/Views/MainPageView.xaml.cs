using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class MainPageView : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPageView(MainPageViewModel viewModel)
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
