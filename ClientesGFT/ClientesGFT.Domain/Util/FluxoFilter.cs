using ClientesGFT.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClientesGFT.Domain.Util
{
    public class FluxoFilter
    {
        public EStatus Status { get; set; }

        [Display(Name = "Cpf")]
        public string CPF { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
