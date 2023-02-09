﻿using SchuetzenOne.Services;
using SchuetzenOne.Views;

namespace SchuetzenOne.ViewModels;

public partial class UserListViewModel : BaseViewModel
{
    public ObservableCollection<User> Users { get; } = new();

    private Random Rand;
    private readonly IUserService _userService;
    public UserListViewModel(IUserService userService)
    {
        _userService = userService;
        Rand = new Random();
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
                user.HasBirthday = DateTime.Today == user.Birthday.Date;
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
    async Task EditUser(User user)
    {
        if (user == null)
            return;

        await Shell.Current.GoToAsync(nameof(UserEditPage), true, new Dictionary<string, object>
        {
            {"User", user }
        });
    }

/*        [RelayCommand]
    async Task OnDateSubmitClicked()
    {
        //var result = await _userService.AddDateAsync(User, TrainingDatePicker.Date);
        //trainingDays.Add(result);
    }*/

/*        [RelayCommand]
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
    async void Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }*/
}