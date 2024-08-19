using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.UserRepository.Abstraction;

public interface IUserRepository
{
    User GetUserByUsername(string userName);
    User GetUserByEmail(string email);
    User GetUserById(Guid id);
    Task<IReadOnlyList<User>> GetAllUser();
    Task<bool> DeleteUser(Guid id);
    Task<bool> AddUser(User user);
    Task<bool> UpdateUser(Guid id, User newUser);
}