using BlazorDashboardApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorDashboardApp.Services
{
    public static class DbInitializer
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync(); // Apply pending migrations

            const string adminName = "admin@admin.admin";
            const string adminEmail = "admin@admin.admin";
            const string adminPassword = "P@ss12345";
            const string adminRoleName = "Admin";
            const string editorRoleName = "Editor";

            // 1. Create roles if not exists
            if (!await roleManager.RoleExistsAsync(adminRoleName))
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            if (!await roleManager.RoleExistsAsync(editorRoleName))
                await roleManager.CreateAsync(new IdentityRole(editorRoleName));

            // 2. Create user if not exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // 3. Add to roles if not already
            if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
            if (!await userManager.IsInRoleAsync(adminUser, editorRoleName))
                await userManager.AddToRoleAsync(adminUser, editorRoleName);

        }
    }
}
