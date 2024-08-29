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
    private readonly IUploadDataRepository _uploadDataRepository;

    public FilePermissionService(IFileUploadedRepository fileUploadedRepository,IUserRepository userRepository, IUserFileRepository userFileRepository, IUploadDataRepository uploadDataRepository)
    {
        _fileUploadedRepository = fileUploadedRepository;
        _userRepository = userRepository;
        _userFileRepository = userFileRepository;
        _uploadDataRepository = uploadDataRepository;
    }


    public async Task<PaginatedFilesDto> GetFilesPagination(int page, int limit)
    {
        var totalFilesCount = await _fileUploadedRepository.GetTotalFilesCountAsync();
        var files = await _fileUploadedRepository.GetFileUploadedInDb(page, limit);

        var paginatedFiles = files.Select(x => new UploadDataDto()
        {
            Id = x.Id,
            FileName = x.FileName,
            Category = x.Category.Name,
            UploadDate = x.UploadDate,
        }).ToList();

        return new PaginatedFilesDto
        {
            Items = paginatedFiles,
            TotalCount = totalFilesCount,
            PageIndex = page
        };
    }

    public async Task<List<UserAccessDto>> GetUserForAccessingFile(string username)
    {
        var users =await _userRepository.GetUsersContainSearchInput(username);
        if (users.Count() == 0)
        {
            throw new UserNotFoundException();
        }
        var result = users.Select(x => new UserAccessDto()
        {
            Id = x.Id.ToString(), UserName = x.Username, FirstName = x.FirstName, LastName = x.LastName
        });
        return result.ToList();
    }
    
    public async Task<IEnumerable<UserWhoAccessThisFileDto>> WhoAccessThisFile(int fileId)
    {
        var users =await _userFileRepository.GetByFileIdAsync(fileId);
        var usersDtos = users.Select(user => new UserWhoAccessThisFileDto()
        {
            Id = user.User.Id,
            UserName = user.User.Username
        });
        return usersDtos;
    }
    
    public async Task AccessFileToUser(List<string> inputUserGuidIds, int fileId)
    {
        var validUserGuids = new List<Guid>();

        foreach (var userId in inputUserGuidIds)
        {
            if (Guid.TryParse(userId, out Guid parsedGuid))
            {
                validUserGuids.Add(parsedGuid);
            }
            else
            {
                throw new GuidNotCorrectFormat();
            }
        }

        foreach (var userGuid in validUserGuids)
        {
            var user = await _userRepository.GetUserById(userGuid);
            if (user is null)
            {
                throw new UserNotFoundException();
            }
        }
        if (await _uploadDataRepository.GetByIdAsync(fileId) is null)
        {
            throw new NoFileUploadedException();
        }
        var currentAccessor = await _userFileRepository.GetUsersIdAccessToInputFile(fileId.ToString());
        var newUsers = validUserGuids.Select(g => g.ToString()).Except(currentAccessor).ToList();
        var blockAccessToFile = currentAccessor.Except(currentAccessor.Intersect(validUserGuids.Select(g => g.ToString()))).ToList();

        await _userFileRepository.RevokeUserAccess(blockAccessToFile);
        await _userFileRepository.GrantUserAccess(newUsers, fileId);
    }
}