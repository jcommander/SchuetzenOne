namespace SchuetzenOne;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

	}

    protected override Window CreateWindow(IActivationState activationState) =>
        new Window(new AppShell())
    {
        // Manipulate Window object
        MinimumHeight = 850,
        MinimumWidth = 700,

        Height = 850,
        Width = 700,

        X= 100,
        Y= 0

    };
}
