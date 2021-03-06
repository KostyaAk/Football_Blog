﻿using football_blog.Models;  // пространство имен модели User
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace football_blog.Models
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "kostya_akulich_2000@mail.ru";
            string password = "Akula!228";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("follower") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("follower"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
