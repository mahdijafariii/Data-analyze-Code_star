namespace AnalysisData.User.Services.ValidationService.Abstraction;

public interface IValidationService
{
    public void EmailCheck(string email);
    public void PasswordCheck(string password);
    public void PhoneNumberCheck(string phoneNumber);
}