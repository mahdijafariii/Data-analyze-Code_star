using AnalysisData.Exception.UserException;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.RoleRepository.Abstraction;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.AdminService;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;
using AnalysisData.User.UserDto.UserDto;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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

        _userRepository.GetUserByEmailAsync(userRegisterDto.Email).Returns((User)null);
        _userRepository.GetUserByUsernameAsync(userRegisterDto.Username).Returns((User)null);

        _passwordHasher.HashPassword(userRegisterDto.Password).Returns("hashedPassword");

        // Act
        await _sut.RegisterByAdminAsync(userRegisterDto);

        // Assert
        await _userRepository.Received(1).AddUserAsync(Arg.Is<User>(u =>
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
    public async Task ValidateUserRegistrationDataAsync_ShouldThrowPasswordMismatchException_WhenPasswordsDoNotMatch()
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
        _roleRepository.GetRoleByNameAsync(userRegisterDto.RoleName).ThrowsAsync(new PasswordMismatchException());

        // Act
        var action = () => _sut.RegisterByAdminAsync(userRegisterDto);

        // Assert
        await Assert.ThrowsAsync<PasswordMismatchException>(action);
    }

    [Fact]
    public async Task ValidateUserInformation_ShouldThrowDuplicateUserException_WhenUserAlreadyExists()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "existingUsername",
            Email = "existingEmail@gmail.com",
            Password = "SecurePassword123",
            ConfirmPassword = "SecurePassword123",
            FirstName = "First",
            LastName = "Last",
            PhoneNumber = "09123456789",
            RoleName = "admin"
        };
        var role = new Role { RoleName = "admin", RolePolicy = "gold" };
        _roleRepository.GetRoleByNameAsync(userRegisterDto.RoleName.ToLower()).Returns(role);
        var existingUserWithUsername = new User
            { Id = Guid.NewGuid(), Username = "existingUsername", Email = "anotherEmail@gmail.com" };
        var existingUserWithEmail = new User
            { Id = Guid.NewGuid(), Username = "anotherUsername", Email = "existingEmail@gmail.com" };

        _userRepository.GetUserByUsernameAsync(userRegisterDto.Username).Returns(existingUserWithUsername);
        _userRepository.GetUserByEmailAsync(userRegisterDto.Email).Returns(existingUserWithEmail);
        // Act
        var action = () => _sut.RegisterByAdminAsync(userRegisterDto);
        // Assert
        await Assert.ThrowsAsync<DuplicateUserException>(action);
    }
    
    [Fact]
    public async Task CheckExistenceRole_ShouldThrowRoleNotFoundException_WhenRoleDoesNotExist()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "newUser",
            Email = "newUser@gmail.com",
            Password = "SecurePassword123",
            ConfirmPassword = "SecurePassword123",
            FirstName = "First",
            LastName = "Last",
            PhoneNumber = "09123456789",
            RoleName = "nonExistentRole"
        };

        _roleRepository.GetRoleByNameAsync(userRegisterDto.RoleName).ThrowsAsync(new RoleNotFoundException());

        // Act
        var action = () => _sut.RegisterByAdminAsync(userRegisterDto);

        //  Assert
        await Assert.ThrowsAsync<RoleNotFoundException>(action);
    }
}