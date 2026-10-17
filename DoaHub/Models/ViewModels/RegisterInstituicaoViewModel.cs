using System.ComponentModel.DataAnnotations;

namespace DoaHub.Models.ViewModels
{
    public class RegisterInstituicaoViewModel
    {
        [Required(ErrorMessage = "Informe o nome da instituição.")]
        [Display(Name = "Nome da instituição")]
        public string NomeInstituicao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o CNPJ.")]
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecione o tipo.")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = "Hemocentro";

        [Required(ErrorMessage = "Informe o endereço.")]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o telefone de contato.")]
        [Display(Name = "Telefone de contato")]
        public string TelefoneContato { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o e-mail de contato.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [Display(Name = "E-mail de contato")]
        public string EmailContato { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o responsável.")]
        [Display(Name = "Responsável")]
        public string Responsavel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o cargo.")]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe uma senha.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos {2} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha de acesso")]
        public string SenhaAcesso { get; set; } = string.Empty;
    }
}
