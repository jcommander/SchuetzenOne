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
        if (viewModel.GetUsersCommand.CanExecute(null))
            viewModel.GetUsersCommand.Execute(null);

        //base.OnNavigatedTo(args);
    }
}

