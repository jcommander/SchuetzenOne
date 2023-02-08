using Microsoft.Extensions.DependencyInjection.Extensions;
using SchuetzenOne.Services;
using SchuetzenOne.ViewModels;
using SchuetzenOne.Views;

namespace SchuetzenOne;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IUserService, UserService>();

        builder.Services.AddSingleton<UserListPage>();
        builder.Services.AddTransient<UserEditPage>();

        builder.Services.AddTransient<UserDetailViewModel>();
        builder.Services.AddSingleton<UserListViewModel>();


        //SecureStorage.Default.RemoveAll();

        return builder.Build();
    }
}
