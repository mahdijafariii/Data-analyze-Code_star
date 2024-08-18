
using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.UserRepository.Abstraction;

public interface IUserRepository
{
    Task<User> GetUser(string userName);
    Task<IReadOnlyList<User>> GetAllUser();
    bool DeleteUser(string userName);
    Task AddUser(User user);
    Task UpdateUser(User user);
}