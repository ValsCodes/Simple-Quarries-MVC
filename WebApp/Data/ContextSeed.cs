using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Data
{
    public static class ContextSeed
    {
        
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Customer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Tech.ToString()));
        }
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "administrator",
                Email = "admin@gmail.com",
                FirstName = "admin",
                LastName = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Customer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Administrator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Tech.ToString());
                }

            }
        }
    }
}
