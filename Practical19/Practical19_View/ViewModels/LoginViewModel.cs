using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Practical19_View.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter email id.")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Id")]
    [StringLength(maximumLength: 255)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter password.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}