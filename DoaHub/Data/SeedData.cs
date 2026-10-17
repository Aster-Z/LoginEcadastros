using DoaHub.Models;
using Microsoft.AspNetCore.Identity;

namespace DoaHub.Data
{
   
    public static class SeedData
    {
        public static async Task InicializarAsync(IServiceProvider servicos)
        {
            var roleManager = servicos.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var perfil in Roles.Todos)
            {
                if (!await roleManager.RoleExistsAsync(perfil))
                {
                    await roleManager.CreateAsync(new IdentityRole(perfil));
                }
            }
        }
    }
}
