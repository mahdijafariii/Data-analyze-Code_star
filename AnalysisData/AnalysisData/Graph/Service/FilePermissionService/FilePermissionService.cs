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
    private readonly IAccessManagementService _accessManagementService;

    public FilePermissionService(IFileUploadedRepository fileUploadedRepository, IUserRepository userRepository,
        IUserFileRepository userFileRepository, IAccessManagementService accessManagementService)
    {
        _fileUploadedRepository = fileUploadedRepository;
        _userRepository = userRepository;
        _userFileRepository = userFileRepository;
        _accessManagementService = accessManagementService;
    }


    public async Task<PaginatedFileDto> GetFilesAsync(int page, int limit)
    {
        var files = await _fileUploadedRepository.GetUploadedFilesAsync(page, limit);
        var totalFilesCount = await _fileUploadedRepository.GetTotalFilesCountAsync();
        var paginationFiles = files.Select(x => new FileEntityDto()
        {
            Id = x.Id, FileName = x.FileName, Category = x.Category.Name, UploadDate = x.UploadDate,
        }).ToList();
        return new PaginatedFileDto()
        {
            Items = paginationFiles,
            TotalCount = totalFilesCount,
            PageIndex = page
        };
    }

    public async Task<List<UserAccessDto>> GetUserForAccessingFileAsync(string username)
    {
        var users = await _userRepository.GetTopUsersByUsernameSearchAsync(username);
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

    public async Task<IEnumerable<WhoAccessThisFileDto>> WhoAccessThisFileAsync(int fileId)
    {
        var userFiles = await _userFileRepository.GetByFileIdAsync(fileId);
        var usersDto = userFiles.Select(user => new WhoAccessThisFileDto()
        {
            Id = user.User.Id,
            UserName = user.User.Username
        });
        return usersDto;
    }

    public async Task AccessFileToUserAsync(List<string> inputUserIdes, int fileId)
    {
        var validUserGuids = new List<Guid>();

        await CheckGuidOfUsersAsync(inputUserIdes, validUserGuids);

        foreach (var userId in validUserGuids)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null)
            {
                throw new UserNotFoundException();
            }
        }

        if (await _fileUploadedRepository.GetByIdAsync(fileId) is null)
        {
            throw new NoFileUploadedException();
        }

        var currentAccessor = await _userFileRepository.GetUserIdsWithAccessToFileAsync(fileId);
        var newUsers = validUserGuids.Select(g => g.ToString()).Except(currentAccessor).ToList();
        var blockAccessToFile = currentAccessor
            .Except(currentAccessor.Intersect(validUserGuids.Select(g => g.ToString()))).ToList();
        await _accessManagementService.RevokeUserAccessAsync(blockAccessToFile);
        await _accessManagementService.GrantUserAccessAsync(newUsers, fileId);
    }

    private async Task CheckGuidOfUsersAsync(List<string> inputUserIdes, List<Guid> validUserGuids)
    {
        foreach (var userId in inputUserIdes)
        {
            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
            if (Guid.TryParse(userId, out Guid parsedGuid))
            {
                validUserGuids.Add(parsedGuid);
            }
            else
            {
                throw new GuidNotCorrectFormat();
            }
        }
    }
}