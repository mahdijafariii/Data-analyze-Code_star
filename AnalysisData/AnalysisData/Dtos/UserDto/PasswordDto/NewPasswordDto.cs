namespace AnalysisData.Dtos.UserDto.PasswordDto;

public class NewPasswordDto
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}