using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Practical17.Models;

namespace Practical17.Data
{
    public class MyDBContext : IdentityDbContext<ApplicationUser>
    {
        public MyDBContext (DbContextOptions<MyDBContext> options): base(options) { }
        
        public DbSet<StudentModel> Students { get; set; } = default!;
    }
}