namespace Domain.Entities;

public class SystemManager
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}