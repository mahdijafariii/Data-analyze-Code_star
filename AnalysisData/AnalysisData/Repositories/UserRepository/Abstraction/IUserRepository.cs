using AnalysisData.Models.UserModel;

namespace AnalysisData.Repositories.UserRepository.Abstraction;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string userName);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
    Task<User> GetUserByIdAsync(Guid id);
    Task<List<User>> GetAllUserPaginationAsync(int page, int limit);
    Task<bool> DeleteUserAsync(Guid id);
    Task<bool> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(Guid id, User newUser);
    Task<int> GetUsersCountAsync();
    Task<IEnumerable<User>> GetTopUsersByUsernameSearchAsync(string username);
}