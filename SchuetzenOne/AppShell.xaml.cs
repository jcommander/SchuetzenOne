using SchuetzenOne.Views;

namespace SchuetzenOne;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Application.Current.UserAppTheme = AppTheme.Dark;
        Routing.RegisterRoute(nameof(UserEditPage), typeof(UserEditPage));
    }
}
