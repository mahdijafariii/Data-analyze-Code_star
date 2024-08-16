using System.Text.RegularExpressions;
using AnalysisData.Exception;
using AnalysisData.UserManage.Model;

namespace AnalysisData.Services;

public class RegexService : IRegexService
{
    public void EmailCheck(string email)
    {
        string pattern = RegexPatterns.EmailRegex;

        Regex regex = new Regex(pattern);
        bool isMatch = regex.IsMatch(email);
        if (!isMatch)
        {
            throw new InvalidEmailFormatException();
        }
    }

    public void PhoneNumberCheck(string phoneNumber)
    {
        string pattern = RegexPatterns.PhoneNumberRegex;
        Regex regex = new Regex(pattern);
        bool isMatch = regex.IsMatch(phoneNumber);
        if (!isMatch)
        {
            throw new InvalidPhoneNumberFormatException();
        }
    }

    public void PasswordCheck(string password)
    {
        string pattern = RegexPatterns.PasswordRegex;
        Regex regex = new Regex(pattern);
        bool isMatch = regex.IsMatch(password);
        if (!isMatch)
        {
            throw new InvalidPasswordFormatException();
        }

    }
}