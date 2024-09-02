using AnalysisData.Data;
using AnalysisData.Model;
using Microsoft.EntityFrameworkCore;

namespace TestProject.User.Repository.UserRepository;

public class UserRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly AnalysisData.Repository.UserRepository.UserRepository _sut;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestUserDatabase").Options;
        _context = new ApplicationDbContext(options);
        _sut = new AnalysisData.Repository.UserRepository.UserRepository(_context);
    }

    private async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    private async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldReturnsUserWithInputUsername_WhenUserWithInputUsernameExists()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Model.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByUsernameAsync("test");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result.Username);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldReturnsNull_WhenUserWithInputUsernameDoesNotExist()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Model.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByUsernameAsync("nonExistentUser");

        // Assert
        Assert.Null(result);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnsUserWithInputEmail_WhenUserWithInputEmailExists()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Model.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByEmailAsync("test@gmail.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@gmail.com", result.Email);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnsNull_WhenUserWithInputEmailDoesNotExist()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Model.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByEmailAsync("nonExistentUser@gmail.com");

        // Assert
        Assert.Null(result);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnsUserWithInputId_WhenUserWithInputIdExists()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Model.User
        {
            Id = userId, Username = "test", Password = "@Test1234",
            Email = "test@gmail.com", FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnsNull_WhenUserWithInputIdDoesNotExist()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Model.User
        {
            Id = userId, Username = "test", Password = "@Test1234",
            Email = "test@gmail.com", FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetAllUserPaginationAsync_ShouldReturnsPaginatedResults_WhenUsersExist()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var users = new List<AnalysisData.Model.User>
        {
            new()
            {
                Username = "user1", Password = "@Test11234",
                Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
                PhoneNumber = "09111111111", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user2", Password = "@Test21234",
                Email = "user2@gmail.com", FirstName = "user2", LastName = "user2",
                PhoneNumber = "09111111112", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user3", Password = "@Test31234",
                Email = "user3@gmail.com", FirstName = "user3", LastName = "user3",
                PhoneNumber = "09111111113", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user4", Password = "@Test41234",
                Email = "user4@gmail.com", FirstName = "user4", LastName = "user4",
                PhoneNumber = "09111111114", ImageURL = null, Role = role
            },
        };

        _context.Roles.Add(role);
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        var page = 1;
        var limit = 2;

        // Act
        var result = await _sut.GetAllUserPaginationAsync(page, limit);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("user3", result[0].Username);
        Assert.Equal("user4", result[1].Username);
        await DisposeAsync();
    }

    [Fact]
    public async Task
        GetAllUserPaginationAsync_ShouldReturnsAllUsers_WhenPageIsZeroAndLimitIsGreaterThanNumberOfExistingUsers()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var users = new List<AnalysisData.Model.User>
        {
            new()
            {
                Username = "user1", Password = "@Test11234",
                Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
                PhoneNumber = "09111111111", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user2", Password = "@Test21234",
                Email = "user2@gmail.com", FirstName = "user2", LastName = "user2",
                PhoneNumber = "09111111112", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user3", Password = "@Test31234",
                Email = "user3@gmail.com", FirstName = "user3", LastName = "user3",
                PhoneNumber = "09111111113", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user4", Password = "@Test41234",
                Email = "user4@gmail.com", FirstName = "user4", LastName = "user4",
                PhoneNumber = "09111111114", ImageURL = null, Role = role
            },
        };

        _context.Roles.Add(role);
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        var page = 0;
        var limit = 10;

        // Act
        var result = await _sut.GetAllUserPaginationAsync(page, limit);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetAllUserPaginationAsync_ShouldReturnsEmptyList_WhenPageIsOutOfRange()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var users = new List<AnalysisData.Model.User>
        {
            new()
            {
                Username = "user1", Password = "@Test11234",
                Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
                PhoneNumber = "09111111111", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user2", Password = "@Test21234",
                Email = "user2@gmail.com", FirstName = "user2", LastName = "user2",
                PhoneNumber = "09111111112", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user3", Password = "@Test31234",
                Email = "user3@gmail.com", FirstName = "user3", LastName = "user3",
                PhoneNumber = "09111111113", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user4", Password = "@Test41234",
                Email = "user4@gmail.com", FirstName = "user4", LastName = "user4",
                PhoneNumber = "09111111114", ImageURL = null, Role = role
            },
        };

        _context.Roles.Add(role);
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        var page = 3;
        var limit = 2;

        // Act
        var result = await _sut.GetAllUserPaginationAsync(page, limit);

        // Assert
        Assert.Empty(result);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetUsersCountAsync_ShouldReturnsCountsOfUsers_Whenever()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var users = new List<AnalysisData.Model.User>
        {
            new()
            {
                Username = "user1", Password = "@Test11234",
                Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
                PhoneNumber = "09111111111", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user2", Password = "@Test21234",
                Email = "user2@gmail.com", FirstName = "user2", LastName = "user2",
                PhoneNumber = "09111111112", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user3", Password = "@Test31234",
                Email = "user3@gmail.com", FirstName = "user3", LastName = "user3",
                PhoneNumber = "09111111113", ImageURL = null, Role = role
            },
            new()
            {
                Username = "user4", Password = "@Test41234",
                Email = "user4@gmail.com", FirstName = "user4", LastName = "user4",
                PhoneNumber = "09111111114", ImageURL = null, Role = role
            },
        };

        _context.Roles.Add(role);
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUsersCountAsync();

        // Assert
        Assert.Equal(4, result);
        await DisposeAsync();
    }


    [Fact]
    public async Task DeleteUserAsync_ShouldReturnsDeleteUserWithInputUserId_WhenUserWithInputUserIdIsExist()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Model.User
        {
            Id = userId, Username = "user1", Password = "@Test11234",
            Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };

        _context.Roles.Add(role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.DeleteUserAsync(userId);

        // Assert
        Assert.True(result);
        Assert.Equal(0, _context.Users.Count());
        await DisposeAsync();
    }


    [Fact]
    public async Task DeleteUserAsync_ShouldReturnsNull_WhenUserWithInputUserIdIsDoesNotExist()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Model.User
        {
            Id = userId, Username = "user1", Password = "@Test11234",
            Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };

        _context.Roles.Add(role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.DeleteUserAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
        Assert.Equal(1, _context.Users.Count());
        await DisposeAsync();
    }

    [Fact]
    public async Task AddUserAsync_ShouldAddsUserToDatabase()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Model.User
        {
            Id = userId, Username = "user1", Password = "@Test11234",
            Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.AddUserAsync(user);

        // Assert
        Assert.Equal(1, _context.Users.Count());
        Assert.Contains(_context.Users, u => u.Username == user.Username);
        await DisposeAsync();
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnTrue_AndUpdateUser_WhenUserExists()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Model.User
        {
            Id = userId, Username = "user1", Password = "@Test11234",
            Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };

        _context.Roles.Add(role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        user.FirstName = "newUser";
        user.LastName = "newUser";


        // Act
        var result = await _sut.UpdateUserAsync(userId, user);

        // Assert
        Assert.True(result);
        Assert.Contains(_context.Users, u => u.Id == userId && u is { FirstName: "newUser", LastName: "newUser" });
        await DisposeAsync();
    }

    [Fact]
    public async Task GetTopUsersByUsernameSearchAsync_ShouldReturnsMatchingUsersWhichContainsInputUsernameAndDoesNotBeDataAnalyst_WhenUsersDoNotBeDataAnalystAndContainsInputUsernameExist()
    {
        // Arrange
        await InitializeAsync();
        var roleAdmin = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var roleManager = new Role { Id = 2, RoleName = "Manager", RolePolicy = "silver" };
        var roleAnalyst = new Role { Id = 3, RoleName = "dataanalyst", RolePolicy = "bronze" };

        var users = new List<AnalysisData.Model.User>
        {
            new()
            {
                Username = "usertestname", Password = "@Test11234",
                Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
                PhoneNumber = "09111111111", ImageURL = null, Role = roleAdmin
            },
            new()
            {
                Username = "testuser2", Password = "@Test21234",
                Email = "user2@gmail.com", FirstName = "user2", LastName = "user2",
                PhoneNumber = "09111111112", ImageURL = null, Role = roleManager
            },
            new()
            {
                Username = "usertest3", Password = "@Test31234",
                Email = "user3@gmail.com", FirstName = "user3", LastName = "user3",
                PhoneNumber = "09111111113", ImageURL = null, Role = roleAnalyst
            }
        };

        _context.Roles.AddRange(roleAdmin, roleManager, roleAnalyst);
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetTopUsersByUsernameSearchAsync("test");

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.DoesNotContain(resultList, u => u.Username == "usertest3");
        await DisposeAsync();
    }

    [Fact]
    public async Task GetTopUsersByUsernameSearchAsync_ShouldReturnEmptyList_WhenNoUsersMatch()
    {
        // Arrange
        await InitializeAsync();
        var roleAdmin = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        var roleManager = new Role { Id = 2, RoleName = "Manager", RolePolicy = "silver" };
        var roleAnalyst = new Role { Id = 3, RoleName = "dataanalyst", RolePolicy = "bronze" };

        var users = new List<AnalysisData.Model.User>
        {
            new()
            {
                Username = "usertestname", Password = "@Test11234",
                Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
                PhoneNumber = "09111111111", ImageURL = null, Role = roleAdmin
            },
            new()
            {
                Username = "testuser2", Password = "@Test21234",
                Email = "user2@gmail.com", FirstName = "user2", LastName = "user2",
                PhoneNumber = "09111111112", ImageURL = null, Role = roleManager
            },
            new()
            {
                Username = "usertest3", Password = "@Test31234",
                Email = "user3@gmail.com", FirstName = "user3", LastName = "user3",
                PhoneNumber = "09111111113", ImageURL = null, Role = roleAnalyst
            }
        };

        _context.Roles.AddRange(roleAdmin, roleManager, roleAnalyst);
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetTopUsersByUsernameSearchAsync("NoUser");

        // Assert
        Assert.Empty(result);
        await DisposeAsync();
    }
}