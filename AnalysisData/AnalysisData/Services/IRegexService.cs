namespace AnalysisData.Services;

public interface IRegexService
{
    public bool EmailCheck(string email);
    public bool PasswordCheck(string password);
    public bool PhoneNumberCheck(string phoneNumber);
}