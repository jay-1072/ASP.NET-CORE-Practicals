using System.ComponentModel.DataAnnotations;

namespace Practical17.Utilities;

public class ValidEmailDomainAttribute : ValidationAttribute
{
    private readonly string allowedDomain;

    public ValidEmailDomainAttribute(string allowedDomain)
    {
        this.allowedDomain = allowedDomain;
    }

    public override bool IsValid(object? value)
    {
        string[] subStrings = value.ToString().Split("@");
        return subStrings[1].ToUpper() == allowedDomain.ToUpper();  
    }
}
