using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public UserType role { get; set; }
    
    public SystemManager SystemManager { get; set; }
    public DataManager DataManager { get; set; }
    public DataAnalyst DataAnalyst { get; set; }
}