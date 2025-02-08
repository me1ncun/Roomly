using Microsoft.AspNetCore.Identity;
using Roomly.Rooms.Helpers;

namespace Roomly.Users.Infrastructure.Extensions;

public static class RoleSeederService
{
    public static async Task SeedRolesAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        await RoleSeeder.SeedRolesAsync(roleManager);
    }
}