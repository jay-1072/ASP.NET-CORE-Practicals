using Microsoft.AspNetCore.Mvc;
using Practical17.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Practical17.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Please enter first name.")]
    [Display(Name = "First Name")]
    [DataType(DataType.Text)]
    [StringLength(maximumLength:20)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Please enter last name.")]
    [Display(Name = "Last Name")]
    [DataType(DataType.Text)]
    [StringLength(maximumLength: 20)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter email id.")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Id")]
    [StringLength(maximumLength:255)]
    [Remote(action: "IsEmailExist", controller: "Account")]
    [ValidEmailDomain(allowedDomain: "gmail.com", ErrorMessage = "Email domain must be gmail.com")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter mobile number.")]
    [DataType(DataType.Text)]
    [Display(Name = "Mobile Number")]
    [RegularExpression(pattern: @"^\d+$", ErrorMessage = "Invalid Mobile Number.")]
    [StringLength(maximumLength:10, MinimumLength=10)]
    public string MobileNumber { get; set; }

    [Required(ErrorMessage = "Please enter password.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please enter confirm password.")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
    public string ConfirmPassword { get; set; }
}