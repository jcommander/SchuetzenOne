namespace SchuetzenOne.Views;

public partial class UserEditPage : ContentPage
{
    public UserEditPage(UserDetailViewModel vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }

}