using System.ComponentModel.DataAnnotations;
using AnalysisData.UserManage.Model.Enum;

namespace AnalysisData.UserManage.Model;

public class User
{
    [Key]
    public int Id { get; set; }//guid
    public string UserName { get; set; }
    public string Password { get; set; }
    public UserType role { get; set; }
    
    public SystemManager SystemManager { get; set; }
    public DataManager DataManager { get; set; }
    public DataAnalyst DataAnalyst { get; set; }
}