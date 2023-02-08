namespace SchuetzenOne.Views;

public partial class UserListPage : ContentPage
{
    public UserListPage(UserListViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}

