using SchuetzenOne.Models;

namespace SchuetzenOne.Services
{
    public interface IUserService
    {
        int UpdateFee(User usr);
        List<Department> AllDepartments { get; set; }
        Task<TrainingDays> AddDateAsync(User user, DateTime date);
        Task<List<User>> GetUsersAsync();
        Task<List<User>> GetInactiveUsersAsync();
        Task<User> GetUserAsync(int id, bool getChildren);
        Task<int> SaveUserAsync(User user);
        Task DeleteItemAsync(User user);
        Task<Department> AddDepartmentAsync(User user, Department dep);
        Task<Department> RemoveDepartmentAsync(User user, Department dep);

    }
}
