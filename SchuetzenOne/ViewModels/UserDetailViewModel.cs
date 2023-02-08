using SchuetzenOne.Services;

namespace SchuetzenOne.ViewModels;

[QueryProperty(nameof(User), "User")]
public partial class UserDetailViewModel : BaseViewModel
{
    public ObservableCollection<TrainingDays> TrainingDays { get; set; } = new ObservableCollection<TrainingDays>();

    [ObservableProperty]
    private User _user = new();

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    public DateTime _trainingDatePicker = DateTime.UtcNow;


    private readonly IUserService _userService;
    public UserDetailViewModel(IUserService userService)
    {
        _userService = userService;
    }

    [RelayCommand]
    async Task DateSubmit()
    {
        await _userService.AddDateAsync(User, TrainingDatePicker.Date);
        //trainingDays.Add(result);
    }


    [RelayCommand]
    async Task GetTrainingDays()
    {
        var u = await _userService.GetUserAsync(User.ID);
        User = u;
    }

    [RelayCommand]
    async Task AddUpdateUser()
    {
        if (string.IsNullOrWhiteSpace(User.Name))
        {
            await Shell.Current.DisplayAlert("Name benötigt", "Bitte gebe einen Namen für deinen Schützen ein", "OK");
            return;
        }

        await _userService.SaveUserAsync(User);
        await Shell.Current.DisplayAlert("Student Info Saved", "Record Saved", "OK");
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async void DeleteUser()
    {
        if (User.ID == 0)
            return;

        await _userService.DeleteItemAsync(User);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    static async void Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}

