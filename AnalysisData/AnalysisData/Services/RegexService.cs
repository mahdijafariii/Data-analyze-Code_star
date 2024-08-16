using System.Text.RegularExpressions;

namespace AnalysisData.Services;

public class RegexService : IRegexService
{
    public bool EmailCheck(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        // بررسی مطابقت ایمیل با الگو
        bool isMatch = regex.IsMatch(email);

        // نمایش نتیجه
        if (isMatch)
        {
            Console.WriteLine("ایمیل معتبر است.");
        }
        else
        {
            Console.WriteLine("ایمیل نامعتبر است.");
        }

        return true;
    }
    public bool PhoneNumberCheck(string phoneNumber)
    {
        string pattern = @"^09\d{9}$";
        Regex regex = new Regex(pattern);
        bool isMatch = regex.IsMatch(phoneNumber);
        if (isMatch)
        {
            return true;
        }
        throw new System.Exception();
    }
    
    public bool PasswordCheck(string password)
    {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        Regex regex = new Regex(pattern);
        bool isMatch = regex.IsMatch(password);
        if (isMatch)
        {
            return true;
        }
        throw new System.Exception();
    }
    
}