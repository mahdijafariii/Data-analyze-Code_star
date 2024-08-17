namespace AnalysisData.Services.Abstraction;

public interface IRegexService
{
    public void EmailCheck(string email);
    public void PasswordCheck(string password);
    public void PhoneNumberCheck(string phoneNumber);
}