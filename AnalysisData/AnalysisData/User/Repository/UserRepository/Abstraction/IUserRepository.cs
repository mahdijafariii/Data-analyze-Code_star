using AnalysisData.Model;

namespace AnalysisData.Repository.UserRepository.Abstraction;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string userName);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(Guid id);
    Task<List<User>> GetAllUserPaginationAsync(int page, int limit);
    Task<bool> DeleteUserAsync(Guid id);
    Task<bool> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(Guid id, User newUser);
    Task<int> GetUsersCountAsync();
    Task<IEnumerable<User>> GetTopUsersByUsernameSearchAsync(string username);
}