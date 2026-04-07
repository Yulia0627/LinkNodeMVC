using Microsoft.AspNetCore.Identity;
using LinkNodeDomain.Model;
using System.Threading.Tasks;
using System;

namespace LinkNodeInfrastructure
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            string adminEmail = "admin1@gmail.comadmin1@gmail.com";
            string password = "Qwerty_1";

            string[] roles = new string[] { "admin", "client", "freelancer" };
            foreach (var roleName in roles)
            {
                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true,
                    Name = "Admin",
                    Surname = "System",
                    Country = "Ukraine",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                
                IdentityResult result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"ПОМИЛКА IDENTITY: {error.Code} - {error.Description}");
                    }
                }
            }
        }
    }
}