using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.UserRepository.Abstraction;

public interface IUserRepository
{
    Task<User> GetUserByUsername(string userName);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserById(Guid id);
    Task<List<User>> GetAllUserPagination(int page, int limit);
    Task<bool> DeleteUser(Guid id);
    Task<bool> AddUser(User user);
    Task<bool> UpdateUser(Guid id, User newUser);
    Task<int> GetUsersCount();
    Task<List<User>> GetUsersContainSearchInput(string username);
}