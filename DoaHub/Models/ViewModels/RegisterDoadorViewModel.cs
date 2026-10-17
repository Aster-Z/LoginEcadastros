using System.ComponentModel.DataAnnotations;

namespace DoaHub.Models.ViewModels
{
    public class RegisterDoadorViewModel
    {
        [Required(ErrorMessage = "Informe seu nome completo.")]
        [Display(Name = "Nome completo")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe um e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe um telefone.")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecione o tipo sanguíneo.")]
        [Display(Name = "Tipo sanguíneo")]
        public string TipoSanguineo { get; set; } = "O+";

        [Required(ErrorMessage = "Informe sua cidade.")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecione o estado.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "MG";

        [Required(ErrorMessage = "Informe uma senha.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos {2} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [Display(Name = "Quero receber avisos de campanhas por e-mail")]
        public bool QuerAvisos { get; set; } = true;
    }
}
