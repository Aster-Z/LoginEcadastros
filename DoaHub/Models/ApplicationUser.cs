using Microsoft.AspNetCore.Identity;

namespace DoaHub.Models
{
    // Estende o usuário padrão do ASP.NET Core Identity com campos próprios do DoaHub.
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
    }
}
