using AnalysisData.Data;
using AnalysisData.UserManage.Model;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Repository.UserRepository;

public class UserRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly ApplicationDbContext _context;
    private readonly AnalysisData.Repository.UserRepository.UserRepository _sut;

    public UserRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDatabase").Options;
        _context = new ApplicationDbContext(_options);
        _sut = new AnalysisData.Repository.UserRepository.UserRepository(_context);
    }
    //
    // [Fact]
    // public async Task GetUser_ShouldReturnsUser_WhenUserExists()
    // {
    //     // Arrange
    //     var user = new User { Id = 1, Username = "user1", Password = "pass1", Email = "mahdijm.bb@gmail.com", FirstName = "mahdi", LastName = "jafari", PhoneNumber = "09162390873" };
    //     _context.Users.Add(user);
    //     await _context.SaveChangesAsync();
    //
    //     // Act
    //     var result = await _sut.GetUser("user1");
    //
    //     // Assert
    //     Assert.Equal("user1", result.Username);
    //     _context.Users.Remove(user);
    //     await _context.SaveChangesAsync();
    // }

    [Fact]
    public async Task GetUser_ShouldReturnsNull_WhenUserDoesNotExist()
    {
        // Arrange
        
        // Act
        var result = await _sut.GetUser("NoOne");

        // Assert
        Assert.Null(result);
    }

    // [Fact]
    // public async Task GetAllUser_ShouldReturnsAllUsers_Whenever()
    // {
    //     _context.Users.Add(
    //         new User { Id = 1, Username = "user1", Password = "pass1" , Email = "mahdijm.bb@gmail.com", FirstName = "mahdi", LastName = "jafari", PhoneNumber = "09162390873" }
    //     );
    //     _context.Users.Add(
    //         new User { Id = 2, Username = "user2", Password = "pass2" ,  Email = "mahdijm.bb2@gmail.com", FirstName = "mahdi2", LastName = "jafari2", PhoneNumber = "09162390872"}
    //     );
    //     await _context.SaveChangesAsync();
    //     
    //     // Act
    //     var result = await _sut.GetAllUser();
    //
    //     // Assert
    //     Assert.Equal(2, result.Count);
    //     _context.Users.Remove(result.ElementAt(0));
    //     _context.Users.Remove(result.ElementAt(1));
    //     await _context.SaveChangesAsync();
    // }
    
    [Fact]
    public void DeleteUser_RemovesUser_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user1", Password = "pass1" , Email = "mahdijm.bb@gmail.com", FirstName = "mahdi", LastName = "jafari", PhoneNumber = "09162390873"};
        _context.Users.Add(user);
        _context.SaveChanges();
        
        // Act
        var result = _sut.DeleteUser("user1");

        // Assert
        Assert.True(result);
        Assert.Equal(0, _context.Users.Count());
    }

    [Fact]
    public void DeleteUser_ReturnsFalse_WhenUserDoesNotExist()
    {
        // Arrange
        
        // Act
        var result = _sut.DeleteUser("NoOne");

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_ShouldAddsUserToDatabase_Whenever()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user1", Password = "pass1" , Email = "mahdijm.bb@gmail.com", FirstName = "mahdi", LastName = "jafari", PhoneNumber = "09162390873"};

        // Act
        _sut.AddUser(user);

        // Assert
        Assert.Equal(1, _context.Users.Count());
        Assert.Contains(_context.Users, u => u is { Username: "user1", Password: "pass1" });
        _context.Users.Remove(user);
        _context.SaveChangesAsync();
    }


}