using SQLite;
using SchuetzenOne.Models;
using SQLiteNetExtensionsAsync.Extensions;

namespace SchuetzenOne.Services;

public class UserService : IUserService
{
    SQLiteAsyncConnection Database;
    public UserService()
    {
    }

    public List<Department> AllDepartments { get; set; }

    private async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        Database.Tracer = new Action<string>(q => Debug.WriteLine(q)); // Writes SQL Commands of API to Debug Console
        Database.Trace = true;
        var result = await Database.CreateTablesAsync<User, Department, TrainingDays, UserDepartments, UserTrainings>();
    }

    public async Task<Department> AddDepartmentAsync(User user, Department dep)
    {
        await Init();
        /*        Department depRec = null; //= await Database.Table<TrainingDays>().Where(i => i.Date == date).FirstOrDefaultAsync();
                if (depRec == null)
                {
                    var newRec = new Department { Name = "Neue Abteilung! Pew Pew", Fee = 20 };
                    var result = await Database.InsertAsync(newRec);
                    depRec = newRec;

                    dateRec = await Database.Table<TrainingDays>().Where(i => i.ID == newRec.ID).FirstOrDefaultAsync();
                    if (result > 1 || dateRec == null || newRec.ID != dateRec.ID)
                        throw new Exception();
                }
                if (!user.Departments.Any(d => d.ID == depRec.ID))
                {  // Even though the DBMS won't add Duplicates our View(Model) would show them
                    user.Departments.Add(depRec);
                }*/
        if (!user.Departments.Any(d => d.ID == dep.ID))
        {
            user.Departments.Add(dep);
            await Database.UpdateWithChildrenAsync(user);
            user.Fee = UpdateFee(user);
        }
       return dep;
    }

    public async Task<Department> RemoveDepartmentAsync(User user, Department dep)
    {
        await Init();
        Department userDep = user.Departments.Where(dpt => dpt.ID == dep.ID).FirstOrDefault();
        if (userDep != null)
        {
            bool rem = user.Departments.Remove(userDep);
            if (rem)
                await Database.UpdateWithChildrenAsync(user);
        }
        user.Fee = UpdateFee(user);
        return dep;
    }


    public async Task<TrainingDays> AddDateAsync(User user, DateTime date)
    {
        await Init();
        var dateRec = await Database.Table<TrainingDays>().Where(i => i.Date == date).FirstOrDefaultAsync();
        if (dateRec == null)
        {
            var newRec = new TrainingDays { Date = date };
            var result = await Database.InsertAsync(newRec);
            dateRec = newRec;
            /*
            dateRec = await Database.Table<TrainingDays>().Where(i => i.ID == newRec.ID).FirstOrDefaultAsync();
            if (result > 1 || dateRec == null || newRec.ID != dateRec.ID)
                throw new Exception();*/
        }
        if (!user.TrainingDays.Any(d => d.ID == dateRec.ID)) {  // Even though the DBMS won't add Duplicates our View(Model) would show them
            user.TrainingDays.Add(dateRec);
        }
        await Database.UpdateWithChildrenAsync(user);
        return dateRec;
    }


    public int UpdateFee(User usr)
    {
        int fee = 0; // default | if no Department

        if (usr.Departments.Count == 1)
            fee = usr.Departments[0].Fee; // get only Department Fee
        else if (usr.Departments.Count > 1)
            fee = 20; // Fee for Multiple Departments TODO: PreProcessor Define

        return fee;
    }


    public async Task<List<User>> GetUsersAsync()
    {
        await Init();

        AllDepartments = await GetDepartmentsAsync(); // We could also assing this in the Init() but maybe we want to add Departments In-App sometime
        //var todos = await Database.Table<TodoItem>().ToListAsync();
        var users = await Database.GetAllWithChildrenAsync<User>();
        foreach (User user in users)
        {
            user.Fee = UpdateFee(user);
            //user.TrainingDays = (ObservableCollection<TrainingDays>)user.TrainingDays.OrderBy(d => d.Date);
            //user.TrainingDays.Sort(tDay => tDay.Date, System.ComponentModel.ListSortDirection.Descending);
            //user.TrainingDays = (ObservableCollection<TrainingDays>)user.TrainingDays.OrderByDescending(tday => tday.Date.Ticks);
            //await Database.GetChildrenAsync(user);
            //user.TrainingDays.OrderByDescending(tday => tday.Date);

            //todos.Add(new TodoItem { Name = user.Name, Notes = user.Email });
        }
        return users;
    }

    public async Task<List<Department>> GetDepartmentsAsync()
    {
        await Init();

        var departments = await Database.Table<Department>().ToListAsync();

        return departments;
    }

    public async Task<List<User>> GetInactiveUsersAsync()
    {
        await Init();
        return await Database.Table<User>().Where(u => !u.Active).ToListAsync();

        // SQL queries are also possible
        //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
    }

    public async Task<User> GetUserAsync(int id, bool getChildren)
    {
        await Init();
        var usr = getChildren ? await Database.GetWithChildrenAsync<User>(id) : await Database.GetAsync<User>(i => i.ID == id);
        return usr;
    }

    public async Task<int> SaveUserAsync(User user)
    {
        await Init();
        if (user.ID != 0)
        {
            return await Database.UpdateAsync(user);
        }
        else
        {
            return await Database.InsertAsync(user);
        }
    }

    public async Task DeleteItemAsync(User user)
    {
        await Init();
        await Database.DeleteAsync(user, true);
    }
}