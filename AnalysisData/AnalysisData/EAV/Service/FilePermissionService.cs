using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Repository.FileUploadedRepository;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Service;

public class FilePermissionService : IFilePermissionService
{
    private readonly IFileUploadedRepository _fileUploadedRepository;
    private readonly IUserRepository _userRepository;
    public FilePermissionService(IFileUploadedRepository fileUploadedRepository,IUserRepository userRepository)
    {
        _fileUploadedRepository = fileUploadedRepository;
        _userRepository = userRepository;
    }


    public async Task<List<UploadDataDto>> GetFilesPagination(int page, int limit)
    {
        var files = await _fileUploadedRepository.GetFileUploadedInDb(page, limit);
        var paginationFiles = files.Select(x => new UploadDataDto()
        {
            Id = x.Id.ToString() ,FileName = x.Name , Category = x.Category, UploadDate = x.UploadDate,
        });
        return paginationFiles.ToList();
    }

    public async Task<List<User>> GetUserForAccessingFile(string username)
    {
        return await _userRepository.GetUsersContainSearchInput(username);
    }
}