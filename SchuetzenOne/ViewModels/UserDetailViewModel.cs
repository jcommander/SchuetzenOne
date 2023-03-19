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

    public List<Department> AllDepartments { get; set; }

    [ObservableProperty]
    public DateTime _trainingDatePicker = DateTime.UtcNow;

    [ObservableProperty]
    public Department _departmentPicker = null;

    private readonly IUserService _userService;
    public UserDetailViewModel(IUserService userService)
    {
        _userService = userService;
        AllDepartments = _userService.AllDepartments; // Get Departments from UserService
        //DepartmentPicker = AllDepartments.First();
    }
    private void UpdateInfo(bool getRegular = false)
    {
        OnPropertyChanging(nameof(User));
        _userService.UpdateUserLocals(User, getRegular);
        OnPropertyChanged(nameof(User));
    }

    [RelayCommand]
    async void AddRemoveTrainingDay(string strRemove = "False")
    {
        bool removeDay = Convert.ToBoolean(strRemove);
        bool hasDay = User.TrainingDays.FirstOrDefault(t => t.Date == TrainingDatePicker.Date) != null; // Contains doesn't work here
        if (removeDay == hasDay)
        {
            Debug.WriteLine((removeDay ? "Removing " : "Adding ") + TrainingDatePicker.Date);
            await _userService.AddRemoveTrainingDayAsync(User, TrainingDatePicker.Date, removeDay);
            UpdateInfo(true);
        }
        //trainingDays.Add(result);
    }

    [RelayCommand]
    async Task AddRemoveDepartment(string strRemove = "False")
    {
        bool removeDep = Convert.ToBoolean(strRemove);
        if (DepartmentPicker is not null)
        {
            bool hasDep = User.Departments.FirstOrDefault(d => d.ID == DepartmentPicker.ID) != null; // Contains doesn't work here
            if (removeDep == hasDep)
            {
                Debug.WriteLine((removeDep ? "Removing " : "Adding ") + DepartmentPicker.Name);
                await _userService.AddRemoveDepartmentAsync(User, DepartmentPicker, removeDep);
                UpdateInfo(false);
            }
        }
        else
            await Shell.Current.DisplayAlert("Abteilung wählen", "Bitte wähle eine Abteilung für deinen Schützen", "OK");

        //trainingDays.Add(result);
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

