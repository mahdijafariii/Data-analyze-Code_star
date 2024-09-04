namespace AnalysisData.User.Model;

public static class RegexPatterns
{
    public const string EmailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
    public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    public const string PhoneNumberRegex = @"^09\d{9}$";
}