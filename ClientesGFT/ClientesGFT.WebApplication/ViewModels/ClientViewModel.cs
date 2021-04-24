using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Util;
using ClientesGFT.WebApplication.Enums;
using ClientesGFT.WebApplication.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClientesGFT.WebApplication.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        public bool HasId { get => Id > 0; }

        [Display(Name = "Status Atual")]
        public EStatus CurrentStatus { get; set; }

        public string CurrentStatusDisplay { get => this.CurrentStatus.GetDisplayName(); }

        [Display(Name = "Internacional?")]
        public bool IsInternational { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [CPFValidation]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [StringLength(12, MinimumLength = 9, ErrorMessage = "RG Inválido.")]
        public string RG { get; set; }
        [Display(Name ="CEP / ZipCode")]
        [StringLength(9, MinimumLength = 5, ErrorMessage = "{0} Inválido.")]
        public string Cep { get; set; }

        [Display(Name = "País")]
        public string Country { get; set; }
        [Required(ErrorMessage = "O campo Pais é obrigatório.")]
        [Range(1, double.MaxValue, ErrorMessage = "País inválido.")]
        public int? CountryId { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }
        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [Range(1, double.MaxValue, ErrorMessage = "Estado inválido.")]
        public int? StateId { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }
        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [Range(1, double.MaxValue, ErrorMessage = "Cidade inválida.")]
        public int? CityId { get; set; }

        [Display(Name = "Rua")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string District { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1, double.MaxValue, ErrorMessage = "Número inválido.")]
        public int Number { get; set; }

        [Display(Name = "Complemento")]
        public string Complement { get; set; }

        [Display(Name= "Última modificação")]
        public DateTime ModifiedDate { get; set; }

        public bool IsEnableToModify { get; set; }

        [MinimumElements(1, ErrorMessage = "É obrigatório ter ao menos 1 telefone.")]
        public List<string> PhonesNumbers { get; set; } = new List<string>();

        public List<Phone> Phones { get; set; } = new List<Phone>();


        public EViewStates ViewState { get; set; } = EViewStates.INSERT;
    }
}
