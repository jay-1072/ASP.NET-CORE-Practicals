using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Practical19_API.Models;

namespace Practical19_API.Data;

public class MyDBContext : IdentityDbContext<ApplicationUser>
{
    public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }
}
