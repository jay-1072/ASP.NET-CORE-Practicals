using Microsoft.AspNetCore.Identity;

namespace Practical19_API.Models;

public class ApplicationUser : IdentityUser
{
    public virtual string? FirstName { get; set; }
    public virtual string? LastName { get; set; }
}
