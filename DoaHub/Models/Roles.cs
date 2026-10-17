namespace DoaHub.Models
{
    // Nomes dos perfis (roles) usados no sistema. Usar estas constantes em vez de
    // strings soltas evita erro de digitação em [Authorize(Roles = ...)].
    public static class Roles
    {
        public const string Doador = "Doador";
        public const string Instituicao = "InstituicaoColetora";

        public static readonly string[] Todos = { Doador, Instituicao };
    }
}
