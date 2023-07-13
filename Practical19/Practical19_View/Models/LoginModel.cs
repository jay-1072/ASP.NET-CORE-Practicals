using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Practical19_View.Models;

public class LoginModel
{
    public string Email { get; set; }

    public string Password { get; set; }

    public bool RememberMe { get; set; }
}