using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Exception.RoleException;
using AnalysisData.Exception.UserException;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.RoleRepository.Abstraction;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.JwtService.Abstraction;
using AnalysisData.Services.ValidationService.Abstraction;
using NSubstitute;

namespace TestProject.User.Services.AdminService;
public class AdminServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;
    private readonly IRoleRepository _roleRepository;
    private readonly IJwtService _jwtService;
    private readonly AnalysisData.Services.UserService.AdminService.AdminService _sut;

    public AdminServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _validationService = Substitute.For<IValidationService>();
        _roleRepository = Substitute.For<IRoleRepository>();
        _jwtService = Substitute.For<IJwtService>();

        _sut = new AnalysisData.Services.UserService.AdminService.AdminService(
            _userRepository,
            _validationService,
            _roleRepository,
            _jwtService);
    }

    [Fact]
    public async Task UpdateUserInformationByAdminAsync_ShouldUpdateUser_WhenInputDataIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Id = userId,
            Username = "Username",
            Password = "@Username123",
            PhoneNumber = "09111111111",
            FirstName = "Username",
            LastName = "Username",
            Email = "Email@gmail.com",
            Role = role
        };

        var updateAdminDto = new UpdateAdminDto
        {
            UserName = "newUsername",
            Email = "newEmail@gmail.com",
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            PhoneNumber = "1234567890",
            RoleName = "Admin"
        };

        _userRepository.GetUserByIdAsync(userId).Returns(user);
        _userRepository.GetUserByUsernameAsync(updateAdminDto.UserName).Returns((AnalysisData.Models.UserModel.User)null);
        _userRepository.GetUserByEmailAsync(updateAdminDto.Email).Returns((AnalysisData.Models.UserModel.User)null);
        _roleRepository.GetRoleByNameAsync(updateAdminDto.RoleName).Returns(role);


        // Act
        await _sut.UpdateUserInformationByAdminAsync(userId, updateAdminDto);

        // Assert
        await _userRepository
            .Received()
            .UpdateUserAsync(userId, Arg.Is<AnalysisData.Models.UserModel.User>(u =>
                u.Username == updateAdminDto.UserName &&
                u.Email == updateAdminDto.Email &&
                u.FirstName == updateAdminDto.FirstName &&
                u.LastName == updateAdminDto.LastName &&
                u.PhoneNumber == updateAdminDto.PhoneNumber &&
                u.Role.RoleName == updateAdminDto.RoleName
            ));

        await _jwtService
            .Received()
            .UpdateUserCookie(updateAdminDto.UserName, false);
    }

    [Fact]
    public async Task ValidateUserInformation_ShouldThrowDuplicateUserException_WhenUsernameOrEmailExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };

        var updateAdminDto = new UpdateAdminDto
        {
            UserName = "existingUsername",
            Email = "existingEmail@test.com"
        };

        var existingUserWithUsername = new AnalysisData.Models.UserModel.User
            { Id = Guid.NewGuid(), Username = "existingUsername", Email = "anotherEmail@gmail.com" };
        var existingUserWithEmail = new AnalysisData.Models.UserModel.User
            { Id = Guid.NewGuid(), Username = "anotherUsername", Email = "existingEmail@gmail.com" };

        _userRepository.GetUserByIdAsync(userId).Returns(user);
        _userRepository.GetUserByUsernameAsync(updateAdminDto.UserName).Returns(existingUserWithUsername);
        _userRepository.GetUserByEmailAsync(updateAdminDto.Email).Returns(existingUserWithEmail);

        // Act
        var action = () => _sut.UpdateUserInformationByAdminAsync(userId, updateAdminDto);

        // Assert
        await Assert.ThrowsAsync<DuplicateUserException>(action);
    }

    [Fact]
    public async Task CheckExistenceOfRole_ShouldThrowRoleNotFoundException_WhenRoleDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };

        var updateAdminDto = new UpdateAdminDto
        {
            RoleName = "NonExistentRole"
        };

        _userRepository.GetUserByIdAsync(userId).Returns(user);
        _roleRepository.GetRoleByNameAsync(updateAdminDto.RoleName).Returns((Role)null);

        // Act
        var action = () => _sut.UpdateUserInformationByAdminAsync(userId, updateAdminDto);

        // Assert
        await Assert.ThrowsAsync<RoleNotFoundException>(action);
    }
    
    [Fact]
    public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.DeleteUserAsync(userId).Returns(true);

        // Act
        var result = await _sut.DeleteUserAsync(userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.DeleteUserAsync(userId).Returns(false);

        // Act
        var action = () => _sut.DeleteUserAsync(userId);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(action);
    }

    [Fact]
    public async Task GetUserCountAsync_ShouldReturnUserCount_WhenCalled()
    {
        // Arrange
        var expectedCount = 5;
        _userRepository.GetUsersCountAsync().Returns(expectedCount);

        // Act
        var result = await _sut.GetUserCountAsync();

        // Assert
        Assert.Equal(expectedCount, result);
    }

    [Fact]
    public async Task GetAllUserAsync_ShouldReturnPaginatedUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<AnalysisData.Models.UserModel.User>
        {
            new()
            {
                Id = Guid.NewGuid(), Username = "user1", FirstName = "First1", LastName = "Last1",
                Email = "email1@gmail.com", PhoneNumber = "09111111111", Role = new Role { RoleName = "Admin" }
            },
            new()
            {
                Id = Guid.NewGuid(), Username = "user2", FirstName = "First2", LastName = "Last2",
                Email = "email2@gmail.com", PhoneNumber = "09222222222", Role = new Role { RoleName = "DataManager" }
            },
        };

        _userRepository.GetAllUserPaginationAsync(0, 2).Returns(users);

        // Act
        var result = await _sut.GetAllUserAsync(0, 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("user1", result[0].UserName);
        Assert.Equal("user2", result[1].UserName);
    }
}