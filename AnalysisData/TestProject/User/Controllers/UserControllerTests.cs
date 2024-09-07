using System.Security.Claims;
using AnalysisData.Controllers.UserController;
using AnalysisData.Dtos.UserDto.PasswordDto;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Services.PermissionService.Abstraction;
using AnalysisData.Services.UserService.UserService.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;

namespace TestProject.User.Controllers;

public class UserControllerTests
{
    private readonly UserController _sut;
    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;
    private readonly IUploadImageService _uploadImageService;
    private readonly IResetPasswordRequestService _resetPasswordRequestService;

    public UserControllerTests()
    {
        _userService = Substitute.For<IUserService>();
        _permissionService = Substitute.For<IPermissionService>();
        _uploadImageService = Substitute.For<IUploadImageService>();
        _resetPasswordRequestService = Substitute.For<IResetPasswordRequestService>();
        _sut = new UserController(_userService, _permissionService, _uploadImageService,_resetPasswordRequestService);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            UserName = "test",
            Password = "Test@123"
        };
        var userDto = new AnalysisData.Models.UserModel.User()
        {
            FirstName = "test",
            LastName = "test",
            ImageURL = "testImageUrl"
        };
        _userService.LoginAsync(userLoginDto).Returns(userDto);

        // Act
        var result = await _sut.Login(userLoginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent =
            JsonConvert.SerializeObject(new { userDto.FirstName, userDto.LastName, userDto.ImageURL });
        Assert.Equal(expectedResponseContent, responseContent);

        await _userService.Received(1).LoginAsync(userLoginDto);
    }

    [Fact]
    public async Task GetPermissions_ShouldReturnOk_WithPermissionsAndUserDetails()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("firstname", "test"),
            new Claim("lastname", "test"),
            new Claim("image", "testImageUrl")
        }));

        var permission = new List<string>()
        {
            "read,write"
        };
        _permissionService.GetPermission(claimsPrincipal).Returns(permission);

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        _sut.HttpContext.User = claimsPrincipal;

        // Act
        var result = await _sut.GetPermissions();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new
        {
            image = "testImageUrl",
            firstName = "test",
            lastName = "test",
            permission
        });
        Assert.Equal(expectedResponseContent, responseContent);

        await _permissionService.Received(1).GetPermission(claimsPrincipal);
    }

    // [Fact]
    // public async Task ResetPassword_ShouldReturnOk_WhenPasswordResetIsSuccessful()
    // {
    //     // Arrange
    //     var resetPasswordDto = new ResetPasswordDto { NewPassword = "NewPass@123", ConfirmPassword = "NewPass@123" };
    //     var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    //     await _userService.ResetPasswordAsync(claimsPrincipal, resetPasswordDto.NewPassword,
    //         resetPasswordDto.ConfirmPassword);
    //
    //     _sut.ControllerContext = new ControllerContext
    //     {
    //         HttpContext = Substitute.For<HttpContext>()
    //     };
    //     _sut.HttpContext.User = claimsPrincipal;
    //
    //     // Act
    //     var result = await _sut.ResetPassword(resetPasswordDto);
    //
    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result);
    //     var responseContent = JsonConvert.SerializeObject(okResult.Value);
    //     var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "success" });
    //     Assert.Equal(expectedResponseContent, responseContent);
    // }
    

    [Fact]
    public async Task UploadImage_ShouldReturnOk_WhenImageIsUploadedSuccessfully()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        fileMock.Length.Returns(1024);
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        _sut.HttpContext.User = claimsPrincipal;

        // Act
        var result = await _sut.UploadImage(fileMock);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "Uploaded successfully." });
        Assert.Equal(expectedResponseContent, responseContent);

        await _uploadImageService.Received(1).UploadImageAsync(claimsPrincipal, fileMock);
    }
    
    // [Fact]
    // public async Task UploadImage_ShouldReturnBadRequest_WhenImageIsNotUploadedSuccessfully()
    // {
    //     // Arrange
    //     var fileMock = Substitute.For<IFormFile>();
    //     fileMock.Length.Returns(0);
    //     var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    //
    //     _sut.ControllerContext = new ControllerContext
    //     {
    //         HttpContext = Substitute.For<HttpContext>()
    //     };
    //     _sut.HttpContext.User = claimsPrincipal;
    //
    //     // Act
    //     var result = await _sut.UploadImage(fileMock);
    //
    //     // Assert
    //     var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    //     var responseContent = JsonConvert.SerializeObject(badRequestResult.Value);
    //     var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "No file uploaded." });
    //     Assert.Equal(expectedResponseContent, responseContent);
    // }

    [Fact]
    public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdatedSuccessfully()
    {
        // Arrange
        var updateUserDto = new UpdateUserDto
        {
            FirstName = "test",
            LastName = "test"
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        await _userService.UpdateUserInformationAsync(claimsPrincipal, updateUserDto);

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        _sut.HttpContext.User = claimsPrincipal;

        // Act
        var result = await _sut.UpdateUser(updateUserDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "updated successfully" });
        Assert.Equal(expectedResponseContent, responseContent);
    }

    [Fact]
    public async Task NewPassword_ShouldReturnOk_WhenPasswordIsResetSuccessfully()
    {
        // Arrange
        var newPasswordDto = new NewPasswordDto
        {
            OldPassword = "OldPass@123",
            NewPassword = "NewPass@123",
            ConfirmPassword = "NewPass@123"
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        await _userService.NewPasswordAsync(claimsPrincipal, newPasswordDto.OldPassword, newPasswordDto.NewPassword,
            newPasswordDto.ConfirmPassword);

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        _sut.HttpContext.User = claimsPrincipal;

        // Act
        var result = await _sut.NewPassword(newPasswordDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "reset successfully" });
        Assert.Equal(expectedResponseContent, responseContent);

    }

    [Fact]
    public async Task GetUserInformation_ShouldReturnOk_WhenUserExist()
    {
        // Arrange
        var userDto = new AnalysisData.Models.UserModel.User()
        {
            FirstName = "test",
            LastName = "test",
            Email = "test@gmail.com",
            PhoneNumber = "09111111111"
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        _sut.HttpContext.User = claimsPrincipal;

        _userService.GetUserAsync(claimsPrincipal).Returns(userDto);

        // Act
        var result = await _sut.GetUserInformation();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new
        {
            FirstName = "test",
            LastName = "test",
            Email = "test@gmail.com",
            PhoneNumber = "09111111111",
            Image = "User do not have information yet !"

        });
        Assert.Equal(expectedResponseContent, responseContent);

        await _userService.Received(1).GetUserAsync(claimsPrincipal);
    }

    [Fact]
    public async Task GetUserInformation_ShouldReturnBadRequest_WhenUserNotExist()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        _userService.GetUserAsync(claimsPrincipal).Returns((AnalysisData.Models.UserModel.User)null);

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        _sut.HttpContext.User = claimsPrincipal;

        // Act
        var result = await _sut.GetUserInformation();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(badRequestResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { message = "not found!" });
        Assert.Equal(expectedResponseContent, responseContent);

        await _userService.Received(1).GetUserAsync(claimsPrincipal);
    }


    [Fact]
    public void Logout_ShouldReturnOk_WhenLogoutIsSuccessful()
    {
        // Arrange
        var response = Substitute.For<HttpResponse>();
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        _sut.HttpContext.Response.Returns(response);

        // Act
        var result = _sut.Logout();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { message = "Logout successful" });
        Assert.Equal(expectedResponseContent, responseContent);

        response.Received(1).Cookies.Delete("AuthToken");
    }
}