using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.FileUploadedRepository;
using AnalysisData.Exception;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Service;

public class FilePermissionService : IFilePermissionService
{
    private readonly IFileUploadedRepository _fileUploadedRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserFileRepository _userFileRepository;

    public FilePermissionService(IFileUploadedRepository fileUploadedRepository,IUserRepository userRepository, IUserFileRepository userFileRepository)
    {
        _fileUploadedRepository = fileUploadedRepository;
        _userRepository = userRepository;
        _userFileRepository = userFileRepository;
    }


    public async Task<List<FileEntityDto>> GetFilesPagination(int page, int limit)
    {
        var files = await _fileUploadedRepository.GetFileUploadedInDb(page, limit);
        var paginationFiles = files.Select(x => new FileEntityDto()
        {
            Id = x.Id.ToString() ,FileName = x.FileName , Category = x.Category.Name, UploadDate = x.UploadDate,
        });
        return paginationFiles.ToList();
    }

    public async Task<List<UserAccessDto>> GetUserForAccessingFile(string username)
    {
        var users =await _userRepository.GetTopUsersByUsernameSearchAsync(username);
        var result = users.Select(x => new UserAccessDto()
        {
            Id = x.Id.ToString(), UserName = x.Username, FirstName = x.FirstName, LastName = x.LastName
        });
        if (!result.Any())
        {
            throw new UserNotFoundException();
        }
        return result.ToList();
    }
    
    public async Task<IEnumerable<UserFile>> WhoAccessThisFile(string fileId)
    {
        var files =await _userFileRepository.GetByFileIdAsync(fileId);
        if (!files.Any())
        {
            throw new UserNotFoundException();
        }
        return files.ToList();
    }
    
    public async Task AccessFileToUser(List<string> inputUserGuidIds,int fileId)
    {
        foreach (var userId in inputUserGuidIds)
        {
            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
            if (user is null)
            {
                throw new UserNotFoundException();
            }
        }
        var currentAccessor = await _userFileRepository.GetUsersIdAccessToInputFile(fileId.ToString());
        var newUsers = inputUserGuidIds.Except(currentAccessor).ToList();
        var blockAccessToFile = currentAccessor.Except(currentAccessor.Intersect(inputUserGuidIds)).ToList();
        await _userFileRepository.RevokeUserAccess(blockAccessToFile);
        await _userFileRepository.GrantUserAccess(newUsers, fileId);
    }
}