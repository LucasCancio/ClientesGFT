using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Util;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Interfaces.Services
{
    public interface IFluxoDeAprovacaoService
    {
        List<Fluxo> ListarFluxo(FluxoFilter filter);
        void AprovarCliente(Client cliente, User usuarioResponsavel);
        void ReprovarCliente(Client cliente, User usuarioResponsavel);
        void EnviarParaCorrecao(Client cliente, User usuarioResponsavel);
    }
}
