using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlineTravelBooking.Domain.Entities.User;
using NetTopologySuite.Geometries;

namespace OnlineTravelBooking.Infrastructure.Persistence.Seed;

public static class ApplicationDbContextSeedExtensions
{
    public static async Task<IApplicationBuilder> SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var services = serviceScope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            logger.LogInformation("Starting database seeding...");

            // Get required services
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed roles and default users
            await SeedRolesAndUsersAsync(roleManager, userManager, logger);

            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }

        return app;
    }

    private static async Task SeedRolesAndUsersAsync(RoleManager<IdentityRole<int>> roleManager, UserManager<ApplicationUser> userManager, ILogger logger)
    {
        var roles = new List<string> { "User", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
                logger.LogInformation("Seeded role: {Role}", role);
            }
        }

        // create a geometry factory with SRID 4326 to produce valid geography values
        var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);

        var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "john.doe",
                    Email = "john.doe@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = new DateOnly(1990, 1, 1),
                    IsActive = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(0,0))
                },
                new ApplicationUser
                {
                    UserName = "jane.smith",
                    Email = "jane.smith@example.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    BirthDate = new DateOnly(1992, 5, 15),
                    IsActive = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(0,0))
                },
                new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    BirthDate = new DateOnly(1992, 5, 15),
                    IsActive = true,
                    PhoneNumber = "+201234567891",
                    Location = geometryFactory.CreatePoint(new Coordinate(0,0))
                }
            };

        foreach (var user in users)
        {
            // You can set a default password for seeding
            if(!userManager.Users.Any(u => u.Email == user.Email))
            {
                var createResult = await userManager.CreateAsync(user, "P@ssword123");
                if (createResult.Succeeded)
                {
                    // mark email confirmed and verified for seeded users
                    user.EmailConfirmed = true;
                    user.IsEmailVerified = true;
                    await userManager.UpdateAsync(user);

                    // Assign roles: admin email -> Admin, others -> User
                    if (string.Equals(user.Email, "admin@example.com", StringComparison.OrdinalIgnoreCase))
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                        logger.LogInformation("Seeded admin user: {Email}", user.Email);
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "User");
                        logger.LogInformation("Seeded user: {Email}", user.Email);
                    }
                }
                else
                {
                    var errorMessage = string.Join("; ", createResult.Errors.Select(e => e.Description));
                    logger.LogWarning("Failed to create seed user {Email}: {Errors}", user.Email, errorMessage);
                }
            }
            else
            {
                // Ensure existing user has correct roles and flags
                var existing = await userManager.FindByEmailAsync(user.Email);
                if (existing != null)
                {
                    var rolesForUser = await userManager.GetRolesAsync(existing);
                    if (string.Equals(existing.Email, "admin@example.com", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!rolesForUser.Contains("Admin"))
                            await userManager.AddToRoleAsync(existing, "Admin");
                        existing.EmailConfirmed = true;
                        existing.IsEmailVerified = true;
                        await userManager.UpdateAsync(existing);
                    }
                    else
                    {
                        if (!rolesForUser.Contains("User"))
                            await userManager.AddToRoleAsync(existing, "User");
                    }
                }
            }
        }
    }
}
