using Microsoft.AspNetCore.Identity;

namespace Practical17.Models;

public static class RoleInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if(!roleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var email = "admin@gmail.com";
        var password = "Admin@123";

        if(userManager.FindByEmailAsync(email).Result == null)
        {
            ApplicationUser user = new()
            {
                Email = email,
                UserName = email,
                FirstName = "Admin",
                LastName = "Admin",
            };
            IdentityResult result = await userManager.CreateAsync(user, password);

            if(result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }
    }
}
