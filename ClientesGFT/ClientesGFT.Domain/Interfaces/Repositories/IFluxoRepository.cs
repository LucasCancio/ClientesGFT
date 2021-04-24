using ClientesGFT.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Interfaces.Repositories
{
    public interface IFluxoRepository
    {
        IList<Fluxo> ListarFluxo(DateTime startDate, DateTime endDate, int statusId=0, string cpf="", string name = "");
        IList<int> ListarIdsClientesIndisponiveis(int userId);
        void EnviarParaGerencia(int clientId, int userId);
        void EnviarParaAprovacao(int clientId, int userId);
        void AprovarCliente(int clientId, int userId);
        void ReprovarCliente(int clientId, int userId);
        void EnviarParaCorrecao(int clientId, int userId);
        bool VerifyIfUserInFluxo(int clientId, int userId);
    }
}
