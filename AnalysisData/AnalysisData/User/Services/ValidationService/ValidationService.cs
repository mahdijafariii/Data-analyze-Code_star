using System.Text.RegularExpressions;
using AnalysisData.Exception;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.Model;

namespace AnalysisData.Services;

public class ValidationService : IValidationService
{
    public void EmailCheck(string email)
    {
        var pattern = RegexPatterns.EmailRegex;

        var regex = new Regex(pattern);
        var isMatch = regex.IsMatch(email);
        if (!isMatch)
        {
            throw new InvalidEmailFormatException();
        }
    }

    public void PhoneNumberCheck(string phoneNumber)
    {
        var pattern = RegexPatterns.PhoneNumberRegex;
        var regex = new Regex(pattern);
        var isMatch = regex.IsMatch(phoneNumber);
        if (!isMatch)
        {
            throw new InvalidPhoneNumberFormatException();
        }
    }

    public void PasswordCheck(string password)
    {
        var pattern = RegexPatterns.PasswordRegex;
        var regex = new Regex(pattern);
        var isMatch = regex.IsMatch(password);
        if (!isMatch)
        {
            throw new InvalidPasswordFormatException();
        }
    }
}