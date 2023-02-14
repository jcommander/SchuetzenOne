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

    public List<Department> AllDepartments { get; set; } = new List<Department>();

    [ObservableProperty]
    public DateTime _trainingDatePicker = DateTime.UtcNow;

    [ObservableProperty]
    public Department _departmentPicker = null;

    string entryText = "";

    private readonly IUserService _userService;
    public UserDetailViewModel(IUserService userService)
    {
        _userService = userService;
        AllDepartments = _userService.AllDepartments; // Get Departments from UserService
    }

    [RelayCommand]
    async Task DateSubmit()
    {
        await _userService.AddDateAsync(User, TrainingDatePicker.Date);
        updateInfo(true);
        //trainingDays.Add(result);
    }

    private void updateInfo(bool getRegular = false)
    {
        OnPropertyChanging(nameof(User));
        _userService.updateUserLocals(User, getRegular);
        OnPropertyChanged(nameof(User));
    }

    [RelayCommand]
    async Task DepartmentSubmit()
    {
        if (DepartmentPicker != null)
        {
            await _userService.AddDepartmentAsync(User, DepartmentPicker);
            updateInfo(false);
        }
        else
            await Shell.Current.DisplayAlert("Abteilung wählen", "Bitte wähle eine Abteilung für deinen Schützen", "OK");

        //trainingDays.Add(result);
    }

    [RelayCommand]
    async Task DepartmentRemove()
    {
        if (DepartmentPicker != null)
        {
            await _userService.RemoveDepartmentAsync(User, DepartmentPicker);
            updateInfo(false);
        }
        else
            await Shell.Current.DisplayAlert("Abteilung wählen", "Bitte wähle eine Abteilung für deinen Schützen", "OK");

        //trainingDays.Add(result);
    }

    [RelayCommand]
    async Task GetTrainingDays()
    {
        var u = await _userService.GetUserAsync(User.ID, true);
        User = u;
    }

    [RelayCommand]
    async Task AddUpdateUser()
    {
        if (User.ID != 0)
        {
            User usr = await _userService.GetUserAsync(User.ID, false);
            if (usr.Name == User.Name && usr.Email == User.Email && usr.Birthday == User.Birthday)
                return;
        }

        if (string.IsNullOrWhiteSpace(User.Name))
        {
            await Shell.Current.DisplayAlert("Name benötigt", "Bitte gebe einen Namen für deinen Schützen ein", "OK");
            return;
        }

        await _userService.SaveUserAsync(User);
        //await Shell.Current.DisplayAlert("Schütze aktualisiert", "Speichern erfolgreich", "OK");
        //await Shell.Current.GoToAsync("..");
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

