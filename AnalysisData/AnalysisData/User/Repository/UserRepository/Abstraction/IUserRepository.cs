namespace AnalysisData.User.Repository.UserRepository.Abstraction;

public interface IUserRepository
{
    Task<Model.User> GetUserByUsernameAsync(string userName);
    Task<Model.User> GetUserByEmailAsync(string email);
    Task<Model.User> GetUserByIdAsync(Guid id);
    Task<List<Model.User>> GetAllUserPaginationAsync(int page, int limit);
    Task<bool> DeleteUserAsync(Guid id);
    Task<bool> AddUserAsync(Model.User user);
    Task<bool> UpdateUserAsync(Guid id, Model.User newUser);
    Task<int> GetUsersCountAsync();
    Task<IEnumerable<Model.User>> GetTopUsersByUsernameSearchAsync(string username);
}