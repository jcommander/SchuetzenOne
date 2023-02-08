using SchuetzenOne.Views;

namespace SchuetzenOne;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(UserEditPage), typeof(UserEditPage));
    }
}
