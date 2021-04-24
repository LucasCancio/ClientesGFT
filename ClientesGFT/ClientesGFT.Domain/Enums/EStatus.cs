using System;
using System.ComponentModel.DataAnnotations;

namespace ClientesGFT.Domain.Enums
{
    public enum EStatus
    {
        [Display(Name = "Todos")]
        TODOS,
        [Display(Name = "Em Cadastro")]
        EM_CADASTRO,
        [Display(Name = "Aguardando Gerência")]
        AGUARDANDO_GERENCIA,
        [Display(Name = "Aguardando Controle de Risco")]
        AGUARDANDO_CONTROLE_DE_RISCO,
        [Display(Name = "Aprovado")]
        APROVADO,
        [Display(Name = "Reprovado")]
        REPROVADO,
        [Display(Name = "Correção de Perfil")]
        CORRECAO_PERFIL
    }
}
