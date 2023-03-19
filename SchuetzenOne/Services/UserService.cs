using CommunityToolkit.Maui.Core.Extensions;
using SQLite;
using SchuetzenOne.Models;
using SQLiteNetExtensionsAsync.Extensions;
using System.Runtime.Intrinsics.Arm;

namespace SchuetzenOne.Services;

public class UserService : IUserService
{
    private SQLiteAsyncConnection _database;
    public UserService()
    {
    }

    public List<Department> AllDepartments { get; set; }

    private async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags)
        {
            Tracer = new Action<string>(q => Debug.WriteLine(q)), // Writes SQL Commands of API to Debug Console
            Trace = true
        };
        var result = await _database.CreateTablesAsync<User, Department, TrainingDays, UserDepartments, UserTrainings>();
        await CheckDefaultData();
    }

    public async Task CheckDefaultData()
    {
        // Departments
        int nrofDeps = await _database.Table<Department>().CountAsync();
        if (nrofDeps != 3)
        {
            // Offline
            await using var stream = await FileSystem.OpenAppPackageFileAsync("DefaultDepartments.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            List<Department> depList = JsonSerializer.Deserialize<List<Department>>(contents);

            await _database.InsertAllAsync(depList);
        }
    }

    #region Locally Calculated Variables
    public void UpdateUserLocals(User user, bool getRegular = false)
    {
        if (getRegular)
            user.Active = IsRegular(user);

        user.HasBirthday = DateTime.Today == user.Birthday.Date;
        user.Fee = GetFee(user);

       user.TrainingDays = user.TrainingDays.OrderBy(tDay => tDay.Date).ToObservableCollection();
    }

    public static int GetFee(User usr)
    {
        switch (usr.Departments.Count)
        {
            case 1:
                return usr.Departments[0].Fee; // get first&only Department Fee
            case > 1:
                return 20; // Fee for Multiple Departments TODO: PreProcessor Define
            default:
                return 0;
        }
    }

    public static bool IsRegular(User usr)
    {
        DateTime today = DateTime.Today;
        DateTime endDate = new DateTime(today.Year, today.Month, 1); // Start Last Month since the current is not usually not finished
        DateTime startDate = endDate.AddYears(-1); // 1 year back from Starting Date

        bool trained18Times = usr.TrainingDays.Count(tDay => tDay.Date > startDate && tDay.Date <= endDate) > 18;

        if (trained18Times)
            return true;

        Debug.WriteLine("-----------------------");
        for (DateTime dt = startDate; dt <= endDate; dt = dt.AddMonths(1))
        {
            bool didTrainInMonth = usr.TrainingDays.Any(tDay => tDay.Date.Month == dt.Month);
            Debug.WriteLine("{0:M/yyyy} - {1} - {2}", dt, usr.Name, didTrainInMonth);
            if (!didTrainInMonth)
            {
                Debug.WriteLine("Inactive!");
                return false;
            }
        }

        return true;
    }
    #endregion

    #region Departments
    public async Task<Department> AddRemoveDepartmentAsync(User user, Department dep, bool remove)
    {
        await Init();
        Department userDep = user.Departments.FirstOrDefault(dpt => dpt.ID == dep.ID);

        if (userDep == null && !remove)
            user.Departments.Add(dep);
        else if (userDep != null && remove)
            user.Departments.Remove(userDep);

        await _database.UpdateWithChildrenAsync(user);
        return dep;
    }

    public async Task<List<Department>> GetDepartmentsAsync()
    {
        await Init();

        var departments = await _database.Table<Department>().ToListAsync();
        return departments;
    }

    #endregion

    #region TrainingDates
    public async Task AddRemoveTrainingDayAsync(User user, DateTime date, bool remove)
    {
        await Init();
        TrainingDays userTDay = user.TrainingDays.FirstOrDefault(tDay => tDay.Date == date.Date);

        if (userTDay == null && !remove)
            user.TrainingDays.Add(new TrainingDays{ Date = date });
        else if (userTDay != null && remove)
            user.TrainingDays.Remove(userTDay);

        await _database.UpdateWithChildrenAsync(user);
    }
    #endregion

    public async Task<List<User>> GetUsersAsync()
    {
        await Init();
        AllDepartments = await GetDepartmentsAsync(); // We could also assing this in the Init() but maybe we want to add Departments In-App sometime
        var users = await _database.GetAllWithChildrenAsync<User>();
        foreach (User user in users)
        {
            UpdateUserLocals(user, true);
        }

        return users;
    }
    public async Task<List<User>> GetInactiveUsersAsync()
    {
        await Init();
        return await _database.Table<User>().Where(u => !u.Active).ToListAsync();

        // SQL queries are also possible
        //return await _database.QueryAsync<User>("SELECT * FROM [User] WHERE [Active] = 0");
    }

    public async Task<User> GetUserAsync(int id, bool getChildren)
    {
        await Init();
        var usr = getChildren ? await _database.GetWithChildrenAsync<User>(id) : await _database.GetAsync<User>(i => i.ID == id);
        return usr;
    }

    public async Task<int> SaveUserAsync(User user)
    {
        await Init();
        if (user.ID == 0)
            return await _database.InsertAsync(user);
        else
            return await _database.UpdateAsync(user);
    }

    public async Task DeleteItemAsync(User user)
    {
        await Init();
        await _database.DeleteAsync(user, true);
    }
}