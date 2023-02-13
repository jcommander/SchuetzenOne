namespace SchuetzenOne.Views;

public partial class UserListPage : ContentPage
{
    public UserListPage(UserListViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        UserListViewModel viewModel = (UserListViewModel)BindingContext;
        viewModel.Refresh();

        base.OnNavigatedTo(args);
    }
}

