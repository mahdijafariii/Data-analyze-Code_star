using AnalysisData.Controllers.UserController;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Exception.PasswordException;
using AnalysisData.Exception.UserException;
using AnalysisData.Services.UserService.AdminService.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;

namespace TestProject.User.Controllers;

public class AdminControllerTests
{
    private readonly AdminController _sut;
    private readonly IAdminService _adminService;
    private readonly IAdminRegisterService _adminRegisterService;

    public AdminControllerTests()
    {
        _adminService = Substitute.For<IAdminService>();
        _adminRegisterService = Substitute.For<IAdminRegisterService>();
        _sut = new AdminController(_adminService, _adminRegisterService);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnOk_WhenUserIsDeletedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _adminService.DeleteUserAsync(userId).Returns(true);

        // Act
        var result = await _sut.DeleteUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { message = "User deleted successfully." });
        Assert.Equal(expectedResponseContent, responseContent);

        await _adminService.Received(1).DeleteUserAsync(userId);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnUsers_WhenUsersExist()
    {
        // Arrange
        var usersPagination = new List<UserPaginationDto>
        {
            new()
            {
                Guid = Guid.NewGuid().ToString(),
                UserName = "test"
            }
        };
        _adminService.GetAllUserAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(usersPagination);
        _adminService.GetUserCountAsync().Returns(10);

        // Act
        var result = await _sut.GetAllUsers(0, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new
        {
            users = usersPagination,
            count = 10,
            thisPage = 0
        });
        Assert.Equal(expectedResponseContent, responseContent);

        await _adminService.Received(1).GetAllUserAsync(0, 10);
        await _adminService.Received(1).GetUserCountAsync();
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdatedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateAdminDto = new UpdateAdminDto { UserName = "test" };

        // Act
        var result = await _sut.UpdateUser(userId, updateAdminDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "updated successfully" });
        Assert.Equal(expectedResponseContent, responseContent);

        await _adminService.Received(1).UpdateUserInformationByAdminAsync(userId, updateAdminDto);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenUserIsRegisteredSuccessfully()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            UserName = "test",
            Email = "test@gmail.com",
            Password = "Test@123",
            ConfirmPassword = "Test@123"
        };

        // Act
        var result = await _sut.Register(userRegisterDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "User added successfully" });
        Assert.Equal(expectedResponseContent, responseContent);

        await _adminRegisterService.Received(1).RegisterByAdminAsync(userRegisterDto);
    }

    [Fact]
    public async Task Register_ShouldThrowPasswordMismatchException_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            UserName = "test",
            Email = "test@gmail.com",
            Password = "Test@123",
            ConfirmPassword = "WrongPassword"
        };
        _adminRegisterService
            .When(x => x.RegisterByAdminAsync(Arg.Any<UserRegisterDto>()))
            .Do(x => throw new PasswordMismatchException());

        // Act
        var action = () => _sut.Register(userRegisterDto);
        
        // Assert
        await Assert.ThrowsAsync<PasswordMismatchException>(action);
        await _adminRegisterService.Received(1).RegisterByAdminAsync(userRegisterDto);
    }
}