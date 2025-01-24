using Common.Constants;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await AddRolesAsync(roleManager);
            await AddAdminAsync(userManager, roleManager);
        }
        private static async Task AddRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues<UserRoles>())
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    _ = await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }
            }
        }
        private static async Task AddAdminAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.FindByEmailAsync("admin@admin.com") is null)
            {
                var user = new User
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, "Admin123@");
                if (!result.Succeeded)
                    throw new Exception("Admin can't be created");

                var role = await roleManager.FindByNameAsync("Admin");
                if (role?.Name == null)
                {
                    throw new Exception("Admin role doesn't exists");
                }
                var addRoleToUser = await userManager.AddToRoleAsync(user, role.Name);
                if (!addRoleToUser.Succeeded)
                    throw new Exception("Cannot assign admin role to user");
            }
        }
    }
}
