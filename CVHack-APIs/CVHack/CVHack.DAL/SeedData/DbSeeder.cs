using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CVHack.DAL;

public static class DbSeeder
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        // 1. Seed Roles
        string[] roleNames = { "Admin", "JobSeeker" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 2. Seed Admin User
        var adminEmail = "admin@cvhack.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var createPowerUser = await userManager.CreateAsync(newAdmin, "Admin@123");
            if (createPowerUser.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }

        // 3. Seed 2 JobSeeker Users
        var jobSeekers = new[]
        {
            new { Email = "jobseeker1@cvhack.com", FirstName = "Alice", LastName = "Smith" },
            new { Email = "jobseeker2@cvhack.com", FirstName = "Bob", LastName = "Jones" }
        };

        foreach (var js in jobSeekers)
        {
            var jsUser = await userManager.FindByEmailAsync(js.Email);
            if (jsUser == null)
            {
                var newJs = new ApplicationUser
                {
                    UserName = js.Email,
                    Email = js.Email,
                    FirstName = js.FirstName,
                    LastName = js.LastName,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createJs = await userManager.CreateAsync(newJs, "Seeker@123");
                if (createJs.Succeeded)
                {
                    await userManager.AddToRoleAsync(newJs, "JobSeeker");

                    // Create the matching UserProfile
                    var profile = new UserProfile
                    {
                        UserId = newJs.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    dbContext.UserProfiles.Add(profile);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        // 4. Seed Supported Roles (for Job Ingestion)
        // Desired set: (Title, SearchQuery). Reconciled on every startup so the
        // table always matches this list exactly.
        var desiredRoles = new (string Title, string SearchQuery)[]
        {
            ("BackendDotNet",   ".NET Developer"),
            ("Cybersecurity",   "Cybersecurity"),
            ("DevOps",          "DevOps Engineer"),
            ("FrontendAngular", "Angular Developer"),
            ("FrontendReact",   "React Developer"),
        };

        var existingRoles = await dbContext.SupportedRoles.ToListAsync();

        // Add or update the desired roles
        foreach (var (title, searchQuery) in desiredRoles)
        {
            var role = existingRoles.FirstOrDefault(r => r.Title == title);
            if (role is null)
            {
                dbContext.SupportedRoles.Add(new SupportedRole
                {
                    Title = title,
                    SearchQuery = searchQuery,
                    IsActive = true
                });
            }
            else
            {
                role.SearchQuery = searchQuery;
                role.IsActive = true;
            }
        }

        // Remove roles that are no longer in the desired set
        var desiredTitles = desiredRoles.Select(r => r.Title).ToHashSet();
        var rolesToRemove = existingRoles.Where(r => !desiredTitles.Contains(r.Title)).ToList();
        dbContext.SupportedRoles.RemoveRange(rolesToRemove);

        await dbContext.SaveChangesAsync();
    }
}
