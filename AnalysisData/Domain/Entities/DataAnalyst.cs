using Domain.Enums;

namespace Domain.Entities;

public class DataAnalyst
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}