namespace TestProject.Services.PermissionService;

public class PermissionServiceTest
{
    private readonly AnalysisData.Services.PermissionService.PermissionService _sut;

    // public PermissionServiceTest()
    // {
    //     _sut = new PermissionService();
    // }
    
    // [Fact]
    // public void GetPermission_ShouldReturnsPermissions_WhenUserHasRole()
    // {
    //     // Arrange
    //     var role = "Admin";
    //     var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
    //     {
    //         new Claim(ClaimTypes.Role, role)
    //     }));
    //
    //     // Act
    //     var result = _sut.GetPermission(userClaims);
    //
    //     // Assert
    //     var expectedPermissions = new List<string> { "permission1", "permission2", "permission3", "permission4" };
    //         
    //     // Convert result to a list and compare
    //     var resultList = result.ToList();
    //     Assert.Equal(expectedPermissions.Count, resultList.Count);
    //     foreach (var permission in expectedPermissions)
    //     {
    //         Assert.Contains(permission, resultList);
    //     }
    // }
    
    // [Fact]
    // public void GetPermission_ShouldReturnsEmpty_WhenUserHasNoRole()
    // {
    //     // Arrange
    //     var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
    //     {
    //         new Claim(ClaimTypes.Role, string.Empty)
    //     }));
    //
    //     // Act
    //     var result = _sut.GetPermission(userClaims);
    //
    //     // Assert
    //     Assert.Empty(result);
    // }
    //
    // [Fact]
    // public void GetPermission_ShouldReturnsEmpty_WhenRoleHasNoPermissions()
    // {
    //     // Arrange
    //     var role = "Admin";
    //     var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
    //     {
    //         new Claim(ClaimTypes.Role, role)
    //     }));
    //
    //     // Act
    //     var result = _sut.GetPermission(userClaims);
    //
    //     // Assert
    //     Assert.Empty(result);
    // }
    //
    //
}