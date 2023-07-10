using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Practical17.Data;
using Practical17.Data.Repository;
using Practical17.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDBContext") ?? throw new InvalidOperationException("Connection string 'MyDBContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepository<StudentModel>, StudentRepository>();

// Add Identity service
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDBContext>();

// Add Authorization filter globally
builder.Services.AddMvc(config => {
    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

    config.Filters.Add(new AuthorizeFilter(policy));
});

var app = builder.Build();

// role seeding

using(var scope = app.Services.CreateScope())
{
    RoleInitializer.InitializeAsync(scope.ServiceProvider).Wait();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
