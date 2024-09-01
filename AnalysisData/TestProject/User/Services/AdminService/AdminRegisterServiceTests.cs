using AnalysisData.Exception;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services;
using AnalysisData.Services.Abstraction;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace TestProject.User.Services.AdminService;

public class AdminRegisterServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly AdminRegisterService _sut;

    public AdminRegisterServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _validationService = Substitute.For<IValidationService>();
        _roleRepository = Substitute.For<IRoleRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();

        _sut = new AdminRegisterService(
            _userRepository,
            _validationService,
            _roleRepository,
            _passwordHasher);
    }

    [Fact]
    public async Task RegisterByAdminAsync_ShouldRegisterUser_WhenDataIsValid()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "newUser",
            Email = "newuser@test.com",
            Password = "Password123",
            ConfirmPassword = "Password123",
            FirstName = "New",
            LastName = "User",
            PhoneNumber = "1234567890",
            RoleName = "Admin"
        };

        var existingRole = new Role { RoleName = "Admin" };
        _roleRepository.GetRoleByNameAsync(Arg.Is("admin")).Returns(existingRole);

        _userRepository.GetUserByEmailAsync(userRegisterDto.Email).Returns((AnalysisData.UserManage.Model.User)null);
        _userRepository.GetUserByUsernameAsync(userRegisterDto.Username).Returns((AnalysisData.UserManage.Model.User)null);

        _passwordHasher.HashPassword(userRegisterDto.Password).Returns("hashedPassword");

        // Act
        await _sut.RegisterByAdminAsync(userRegisterDto);

        // Assert
        await _userRepository.Received(1).AddUserAsync(Arg.Is<AnalysisData.UserManage.Model.User>(u =>
            u.Username == userRegisterDto.Username &&
            u.Email == userRegisterDto.Email &&
            u.FirstName == userRegisterDto.FirstName &&
            u.LastName == userRegisterDto.LastName &&
            u.PhoneNumber == userRegisterDto.PhoneNumber &&
            u.Password == "hashedPassword" &&
            u.Role.RoleName == userRegisterDto.RoleName
        ));

        _validationService.Received(1).EmailCheck(userRegisterDto.Email);
        _validationService.Received(1).PasswordCheck(userRegisterDto.Password);
        _validationService.Received(1).PhoneNumberCheck(userRegisterDto.PhoneNumber);
    }
    [Fact]
    public async Task RegisterByAdminAsync_ShouldThrowPasswordMismatchException_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "newUser",
            Email = "newUser@example.com",
            Password = "SecurePassword123",
            ConfirmPassword = "DifferentPassword",
            FirstName = "First",
            LastName = "Last",
            PhoneNumber = "1234567890",
            RoleName = "admin"
        };
        var role = new Role { RoleName = "admin" };
        _roleRepository.GetRoleByNameAsync(userRegisterDto.RoleName).ThrowsAsync(new PasswordMismatchException());
        
        // Act
        var action = () => _sut.RegisterByAdminAsync(userRegisterDto);

        // Assert
        await Assert.ThrowsAsync<PasswordMismatchException>(action);
    }

    [Fact]
    public async Task RegisterByAdminAsync_ShouldThrowDuplicateUserException_WhenUserAlreadyExists()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "existingUser",
            Email = "existingUser@example.com",
            Password = "SecurePassword123",
            ConfirmPassword = "SecurePassword123",
            FirstName = "First",
            LastName = "Last",
            PhoneNumber = "1234567890",
            RoleName = "admin"
        };
        var role = new Role { RoleName = "admin" };
        _roleRepository.GetRoleByNameAsync(userRegisterDto.RoleName).Returns(Task.FromResult(role));
        
        _userRepository.GetUserByEmailAsync(userRegisterDto.Email).ThrowsAsync(new DuplicateUserException());
        _userRepository.GetUserByUsernameAsync(userRegisterDto.Username).ThrowsAsync(new DuplicateUserException());

        // Act
        var action = () => _sut.RegisterByAdminAsync(userRegisterDto);
        // Assert
        await Assert.ThrowsAsync<DuplicateUserException>(action);
    }

    [Fact]
    public async Task RegisterByAdminAsync_ShouldThrowRoleNotFoundException_WhenRoleDoesNotExist()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "newUser",
            Email = "newUser@example.com",
            Password = "SecurePassword123",
            ConfirmPassword = "SecurePassword123",
            FirstName = "First",
            LastName = "Last",
            PhoneNumber = "1234567890",
            RoleName = "nonExistentRole"
        };

        _roleRepository.GetRoleByNameAsync(userRegisterDto.RoleName).ThrowsAsync(new RoleNotFoundException());

        // Act
        var action = () => _sut.RegisterByAdminAsync(userRegisterDto);

        //  Assert
        await Assert.ThrowsAsync<RoleNotFoundException>(action);
    }
    
    [Fact]
    public async Task AddFirstAdminAsync_ShouldCreateAdminRoleAndUser_WhenAdminDoesNotExist()
    {
            // Arrange
            _userRepository.GetUserByUsernameAsync("admin").Returns((AnalysisData.UserManage.Model.User)null);

            // Act
            await _sut.AddFirstAdminAsync();

            // Assert
            await _roleRepository.Received(1).AddRoleAsync(Arg.Is<Role>(r =>
                r.RoleName == "admin" &&
                r.RolePolicy == "gold"
            ));

            await _userRepository.AddUserAsync(Arg.Is<AnalysisData.UserManage.Model.User>(u =>
                u.Username == "admin" &&
                u.Email == "admin@gmail.com" &&
                u.Password == "hashedPassword" &&
                u.Role.RoleName == "admin"
            ));

            _passwordHasher.Received(1).HashPassword("admin");
    }

    [Fact]
    public async Task AddFirstAdminAsync_ShouldThrowAdminExistenceException_WhenAdminAlreadyExists()
    {
        // Arrange
        _userRepository.GetUserByUsernameAsync("admin").ThrowsAsync(new AdminExistenceException());

        // Act
        var action = () => _sut.AddFirstAdminAsync();

        // Assert
        await Assert.ThrowsAsync<AdminExistenceException>(action);
    }
}