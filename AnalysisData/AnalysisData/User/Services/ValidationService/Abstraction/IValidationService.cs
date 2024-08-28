namespace AnalysisData.Services.Abstraction;

public interface IValidationService
{
    public void EmailCheck(string email);
    public void PasswordCheck(string password);
    public void PhoneNumberCheck(string phoneNumber);
}