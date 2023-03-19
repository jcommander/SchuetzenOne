using SchuetzenOne.Models;

namespace SchuetzenOne.Services
{
    public interface IUserService
    {
        void UpdateUserLocals(User user, bool getRegular = false);
        Task<Department> AddRemoveDepartmentAsync(User user, Department dep, bool remove);
        Task AddRemoveTrainingDayAsync(User user, DateTime date, bool remove);
        List<Department> AllDepartments { get; set; }
        Task<List<User>> GetUsersAsync();
        Task<List<User>> GetInactiveUsersAsync();
        Task<User> GetUserAsync(int id, bool getChildren);
        Task<int> SaveUserAsync(User user);
        Task DeleteItemAsync(User user);

    }
}
