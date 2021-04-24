using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.WebApplication.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesGFT.WebApplication.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public bool HasId { get => Id > 0; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "O Nome precisa ter de {2} a {1} caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "O Usuário precisa ter de {2} a {1} caracteres.")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "A Senha precisa ter de {2} a {1} caracteres.")]
        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }

        [MinimumElements(1, ErrorMessage = "É obrigatório ter ao menos 1 perfil.")]
        public List<int> RolesIds { get; set; } = new List<int>();
        public IList<Role> Roles { get; set; }

        public bool IsEnableToModify { get; set; }
    }
}
