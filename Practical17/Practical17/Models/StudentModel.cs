using System.ComponentModel.DataAnnotations;

namespace Practical17.Models;

public enum GenderList
{
    Male = 1,
    Female = 2
}

public class StudentModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter enrollment number.")]
    [DataType(DataType.Text)]
    [StringLength(maximumLength:12, MinimumLength=12, ErrorMessage = "Enrollment number must be 12 digits long.")]
    [Display(Name = "Enrollment Number")]
    public string EnrollmentNumber { get; set; }

    [Required(ErrorMessage = "Please enter first name.")]
    [DataType(DataType.Text)]
    [StringLength(maximumLength: 20)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Please enter last name.")]
    [DataType(DataType.Text)]
    [StringLength(maximumLength: 20)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter email address.")]
    [DataType(DataType.EmailAddress)]
    [StringLength(maximumLength: 255)]
    [Display(Name = "Email Id")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter date of birth.")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Please select gender.")]
    [DisplayFormat(ApplyFormatInEditMode = true)]
    public GenderList? Gender { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }
}
