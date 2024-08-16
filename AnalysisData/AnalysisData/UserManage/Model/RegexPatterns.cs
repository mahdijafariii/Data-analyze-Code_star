namespace AnalysisData.UserManage.Model;

public static class RegexPatterns
{
    public const string EmailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
    public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    public const string PhoneNumberRegex = @"^09\d{9}$";
}