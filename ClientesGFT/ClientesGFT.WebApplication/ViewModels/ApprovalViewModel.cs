using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using System.Collections.Generic;

namespace ClientesGFT.WebApplication.ViewModels
{
    public class ApprovalViewModel
    {
        public IEnumerable<Client> ClientsToApprove { get; set; }
        public IEnumerable<Client> ApprovedClients { get; set; }
        public IEnumerable<Client> RepprovedClients { get; set; }
        public IList<ERoles> Roles { get; set; }

        public bool CanSeeAproveTable { get => this.Roles.Contains(ERoles.GERENCIA) ||
                                               this.Roles.Contains(ERoles.CONTROLE_DE_RISCO) || 
                                               this.Roles.Contains(ERoles.ADMINISTRACAO); }
    }
}
