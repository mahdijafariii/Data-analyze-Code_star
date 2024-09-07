namespace AnalysisData.Dtos.UserDto.PasswordDto;

public class ResetPasswordDto
{
    public string ResetPasswordToken { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}