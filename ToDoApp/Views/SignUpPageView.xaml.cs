using ToDoApp.ViewModels;

namespace ToDoApp.Views;

public partial class SignUpPageView : ContentPage
{
	public SignUpPageView(SignUpPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}