using SchuetzenOne.Models;

namespace SchuetzenOne.Services
{
    public interface IUserService
    {
        Task<TrainingDays> AddDateAsync(User user, DateTime date);
        Task<List<User>> GetUsersAsync();
        Task<List<User>> GetInactiveUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<int> SaveUserAsync(User user);
        Task<int> DeleteItemAsync(User user);
        Task<Department> AddDepartmentAsync(User user);
    }
}
