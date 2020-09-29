using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UserManagementAPI.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            

        }
        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByEmailAsync("papunsahoo12@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Papun1",
                    Email = "papunsahoo12@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "Passw@rd1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
            if (await userManager.FindByEmailAsync("papunsahoo2012@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "User1",
                    Email = "papunsahoo2012@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "Password1@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
            if (await userManager.FindByEmailAsync("papunsahoo2013@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "user2",
                    Email = "papunsahoo2013@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "Password2@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
        }
        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                var role = new IdentityRole
                {
                    Name = "admin"
                };
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("user"))
            {
                var role = new IdentityRole
                {
                    Name = "user"
                };
                await roleManager.CreateAsync(role);
            }
        }


    }
}
