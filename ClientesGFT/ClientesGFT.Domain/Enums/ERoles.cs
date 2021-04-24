using System.ComponentModel.DataAnnotations;

namespace ClientesGFT.Domain.Enums
{
    public enum ERoles
    {
        [Display(Name = "Administração")]
        ADMINISTRACAO,
        [Display(Name = "Operação")]
        OPERACAO,
        [Display(Name = "Gerência")]
        GERENCIA,
        [Display(Name = "Controle de Risco")]
        CONTROLE_DE_RISCO
    }
}
