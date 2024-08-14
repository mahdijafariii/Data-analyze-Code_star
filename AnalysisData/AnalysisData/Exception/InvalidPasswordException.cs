namespace AnalysisData.Exception;
using System;


public class PasswordIsInvalid : Exception
{
    public PasswordIsInvalid() : base(Resources.PasswordIsInvalid , new ArgumentException())
    {
    }
}