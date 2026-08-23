using ToDoApp.ViewModels;

namespace ToDoApp.Views;

public partial class SignInPageView : ContentPage
{
	public SignInPageView(SignInPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}