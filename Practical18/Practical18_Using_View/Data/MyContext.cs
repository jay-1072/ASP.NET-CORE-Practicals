using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Practical18_Using_View.ViewModels;

namespace Practical18_Using_View.Data
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public DbSet<Practical18_Using_View.ViewModels.StudentViewModel> Students { get; set; } = default!;
    }
}
