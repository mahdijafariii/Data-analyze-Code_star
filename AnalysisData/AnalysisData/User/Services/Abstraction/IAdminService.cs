using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.UpdateModel;

namespace AnalysisData.Services.Abstraction;

public interface IAdminService
{
    Task<bool> Register(UserRegisterModel userRegisterModel);
    Task<IReadOnlyList<User>> GetAllUsers(); 
    Task<bool> UpdateUserInformationByAdmin(Guid id, UpdateAdminModel updateAdminModel);
    Task<bool> DeleteUser(Guid id);
    
    Task AddFirstAdmin();

    
    
}