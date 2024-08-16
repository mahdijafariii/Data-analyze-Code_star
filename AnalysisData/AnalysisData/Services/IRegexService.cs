namespace AnalysisData.Services;

public interface IRegexService
{
    public void EmailCheck(string email);
    public void PasswordCheck(string password);
    public void PhoneNumberCheck(string phoneNumber);
}