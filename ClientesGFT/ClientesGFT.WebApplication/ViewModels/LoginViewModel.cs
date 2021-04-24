using System.ComponentModel.DataAnnotations;

namespace ClientesGFT.WebApplication.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="Usuário")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
