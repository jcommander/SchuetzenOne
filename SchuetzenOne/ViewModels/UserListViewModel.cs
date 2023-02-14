using SchuetzenOne.Services;
using SchuetzenOne.Views;

namespace SchuetzenOne.ViewModels;

public partial class UserListViewModel : BaseViewModel
{
    public ObservableCollection<User> Users { get; set; } = new();

    private Random Rand;
    private readonly IUserService _userService;
    public UserListViewModel(IUserService userService)
    {
        _userService = userService;
        Rand = new Random();
        //Task.Run(async () => await GetUsersAsync());
    }

    public void Refresh()
    {
        Task.Run(async () => await GetUsersAsync());
    }

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    public bool _hasBirthday = false;

    [RelayCommand]
    async Task GetUsersAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var users = await _userService.GetUsersAsync();

            if (Users.Count != 0)
                Users.Clear();

            foreach (var user in users)
            {

                Users.Add(user);
            }

        }
        catch (Exception ex)
        {
            //System::WriteLine($"Unable to get users: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }

    }

    [RelayCommand]
    async Task AddUser()
    {
        await Shell.Current.GoToAsync(nameof(UserEditPage), true);
    }


    [RelayCommand]
    async Task EditUser(User user)
    {
        if (user == null)
            return;

        await Shell.Current.GoToAsync(nameof(UserEditPage), true, new Dictionary<string, object>
        {
            {"User", user }
        });
    }

}