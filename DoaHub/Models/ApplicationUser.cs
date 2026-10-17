using Microsoft.AspNetCore.Identity;

namespace DoaHub.Models
{
    // Estende o usuário padrão do ASP.NET Core Identity com os campos dos dois
    // formulários de cadastro do DoaHub. Nem todo campo se aplica a todo usuário
    // (ex.: Cnpj só é usado por instituições) — fica tudo numa tabela só por
    // simplicidade nesta fase; dá para separar em tabelas de perfil depois.
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;

        // Doador
        public string? Telefone { get; set; }
        public string? TipoSanguineo { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public bool QuerAvisos { get; set; }

        // Instituição coletora
        public string? Cnpj { get; set; }
        public string? TipoInstituicao { get; set; }
        public string? Endereco { get; set; }
        public string? Responsavel { get; set; }
        public string? Cargo { get; set; }
    }
}
