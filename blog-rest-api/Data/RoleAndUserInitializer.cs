using Microsoft.AspNetCore.Identity;

namespace blog_rest_api.Data
{
    public static class RoleAndUserInitializer
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                var adminRole = new IdentityRole("admin");
                await roleManager.CreateAsync(adminRole);

                var bloggerRole = new IdentityRole("blogger");
                await roleManager.CreateAsync(bloggerRole);
            }

            var anyAdminInSystem = await userManager.GetUsersInRoleAsync("admin");

            if (!anyAdminInSystem.Any())
            {
                var admin = new IdentityUser("admin");
                admin.Email = "admin@test.com";
                admin.EmailConfirmed = true;
                var result = await userManager.CreateAsync(admin, "psWD123!");
                await userManager.GenerateEmailConfirmationTokenAsync(admin);
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
