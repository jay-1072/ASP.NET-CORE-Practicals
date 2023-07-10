using Practical18_Using_View.ViewModels;

namespace Practical18_Using_View.DataModels;

public class StudentModel
{
    public int Id { get; set; }

    public string EnrollmentNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public DateTime DateOfBirth { get; set; }

    public GenderList? Gender { get; set; }
}
