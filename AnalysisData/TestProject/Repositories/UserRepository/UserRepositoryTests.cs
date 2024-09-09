using AnalysisData.Data;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


public class UserRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly UserRepository _sut;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _sut = new UserRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldReturnsUserWithInputUsername_WhenUserWithInputUsernameExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        //Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByUsernameAsync("test");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result.Username);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldReturnsNull_WhenUserWithInputUsernameDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        //Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByUsernameAsync("nonExistentUser");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnsUserWithInputEmail_WhenUserWithInputEmailExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        //Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByEmailAsync("test@gmail.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@gmail.com", result.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnsNull_WhenUserWithInputEmailDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        //Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByEmailAsync("nonExistentUser@gmail.com");

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetUserByPhoneNumberAsync_ShouldReturnsUserWithInputPhoneNumber_WhenUserWithInputPhoneNumberExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        //Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByPhoneNumberAsync("09111111111");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result.Username);
    }

    [Fact]
    public async Task GetUserByPhoneNumberAsync_ShouldReturnsNull_WhenUserWithInputPhoneNumberDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        //Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var user = new AnalysisData.Models.UserModel.User
        {
            Username = "test", Password = "@Test1234",
            Email = "test@gmail.com",
            FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByPhoneNumberAsync("09111111112");

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnsUserWithInputId_WhenUserWithInputIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        //Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Models.UserModel.User
        {
            Id = userId, Username = "test", Password = "@Test1234",
            Email = "test@gmail.com", FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnsNull_WhenUserWithInputIdDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        //Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Models.UserModel.User
        {
            Id = userId, Username = "test", Password = "@Test1234",
            Email = "test@gmail.com", FirstName = "test", LastName = "test",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUserByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllUserPaginationAsync_ShouldReturnsPaginatedResults_WhenUsersExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var users = new List<AnalysisData.Models.UserModel.User>
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

        context.Roles.Add(role);
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        var page = 1;
        var limit = 2;

        // Act
        var result = await _sut.GetAllUserPaginationAsync(page, limit);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("user3", result[0].Username);
        Assert.Equal("user4", result[1].Username);
    }

    [Fact]
    public async Task
        GetAllUserPaginationAsync_ShouldReturnsAllUsers_WhenPageIsZeroAndLimitIsGreaterThanNumberOfExistingUsers()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var users = new List<AnalysisData.Models.UserModel.User>
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

        context.Roles.Add(role);
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        var page = 0;
        var limit = 10;

        // Act
        var result = await _sut.GetAllUserPaginationAsync(page, limit);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task GetAllUserPaginationAsync_ShouldReturnsEmptyList_WhenPageIsOutOfRange()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var users = new List<AnalysisData.Models.UserModel.User>
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

        context.Roles.Add(role);
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        var page = 3;
        var limit = 2;

        // Act
        var result = await _sut.GetAllUserPaginationAsync(page, limit);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUsersCountAsync_ShouldReturnsCountsOfUsers_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var users = new List<AnalysisData.Models.UserModel.User>
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

        context.Roles.Add(role);
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUsersCountAsync();

        // Assert
        Assert.Equal(4, result);
    }


    [Fact]
    public async Task DeleteUserAsync_ShouldDeleteUserWithInputUserId_WhenUserWithInputUserIdIsExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Models.UserModel.User
        {
            Id = userId, Username = "user1", Password = "@Test11234",
            Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };

        context.Roles.Add(role);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.DeleteUserAsync(userId);

        // Assert
        Assert.True(result);
        Assert.Equal(0, context.Users.Count());
    }


    [Fact]
    public async Task DeleteUserAsync_ShouldReturnsNull_WhenUserWithInputUserIdIsDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Models.UserModel.User
        {
            Id = userId, Username = "user1", Password = "@Test11234",
            Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };

        context.Roles.Add(role);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.DeleteUserAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
        Assert.Equal(1, context.Users.Count());
    }

    [Fact]
    public async Task AddUserAsync_ShouldAddsUserToDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Models.UserModel.User
        {
            Id = userId, Username = "user1", Password = "@Test11234",
            Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.AddUserAsync(user);

        // Assert
        Assert.True(result);
        Assert.Equal(1, context.Users.Count());
        Assert.Contains(context.Users, u => u.Username == user.Username);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnTrue_AndUpdateUser_WhenUserExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Models.UserModel.User
        {
            Id = userId, Username = "user1", Password = "@Test11234",
            Email = "user1@gmail.com", FirstName = "user1", LastName = "user1",
            PhoneNumber = "09111111111", ImageURL = null, Role = role
        };

        context.Roles.Add(role);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        user.FirstName = "newUser";
        user.LastName = "newUser";


        // Act
        var result = await _sut.UpdateUserAsync(userId, user);

        // Assert
        Assert.True(result);
        Assert.Contains(context.Users, u => u.Id == userId && u is { FirstName: "newUser", LastName: "newUser" });
    }

    [Fact]
    public async Task
        GetTopUsersByUsernameSearchAsync_ShouldReturnsMatchingUsersWhichContainsInputUsernameAndDoesNotBeDataAnalyst_WhenUsersDoNotBeDataAnalystAndContainsInputUsernameExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var roleAdmin = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var roleManager = new Role { RoleName = "Manager", RolePolicy = "silver" };
        var roleAnalyst = new Role { RoleName = "data-analyst", RolePolicy = "bronze" };

        var users = new List<User>
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

        context.Roles.AddRange(roleAdmin, roleManager, roleAnalyst);
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetTopUsersByUsernameSearchAsync("test");

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(1, resultList.Count);
        Assert.Contains(resultList, u => u.Username == "usertest3");
    }

    [Fact]
    public async Task GetTopUsersByUsernameSearchAsync_ShouldReturnEmptyList_WhenNoUsersMatch()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var roleAdmin = new Role { RoleName = "Admin", RolePolicy = "gold" };
        var roleManager = new Role { RoleName = "Manager", RolePolicy = "silver" };
        var roleAnalyst = new Role { RoleName = "data-analyst", RolePolicy = "bronze" };

        var users = new List<User>
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

        context.Roles.AddRange(roleAdmin, roleManager, roleAnalyst);
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetTopUsersByUsernameSearchAsync("NoUser");

        // Assert
        Assert.Empty(result);
    }
}