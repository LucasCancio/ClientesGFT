using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Exceptions;
using ClientesGFT.Domain.Interfaces.Repositories;
using ClientesGFT.Domain.Interfaces.Services;
using ClientesGFT.Domain.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClientesGFT.Domain.Services
{
    public class FluxoDeAprovacaoService : IFluxoDeAprovacaoService
    {
        private readonly IFluxoRepository _fluxoRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        public FluxoDeAprovacaoService(IFluxoRepository fluxoRepository, IClientRepository clientRepository, IUserRepository userRepository)
        {
            _fluxoRepository = fluxoRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
        }

        public List<Fluxo> ListarFluxo(FluxoFilter filter)
        {
            if (filter == null || filter.StartDate == DateTime.MinValue || filter.EndDate == DateTime.MinValue)
            {
                filter = new FluxoFilter()
                {
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now
                };
            }

            filter.CPF = DocumentFixer.Fix(filter.CPF);
            var statusId = EnumHelper.StatusParaStatusId(filter.Status);

            var fluxos = _fluxoRepository.ListarFluxo(filter.StartDate, filter.EndDate, statusId, filter.CPF, filter.Name)
                                                                                                        .OrderByDescending(f => f.CreateDate)
                                                                                                        .ToList();
            return fluxos;
        }


        public void AprovarCliente(Client cliente, User usuarioResponsavel)
        {
            ValidateClientAndUser(cliente.Id, usuarioResponsavel.Id);

            if (cliente.CurrentStatus.Description == EStatus.EM_CADASTRO ||
                cliente.CurrentStatus.Description == EStatus.CORRECAO_PERFIL)
            {
                EnviarParaGerencia(cliente, usuarioResponsavel);
            }
            else if (cliente.IsInternacional && cliente.CurrentStatus.Description == EStatus.AGUARDANDO_CONTROLE_DE_RISCO)
            {
                ValidateFluxo(cliente.Id, usuarioResponsavel.Id);
                AprovarClienteInternacional(cliente, usuarioResponsavel);
            }
            else
            {
                ValidateFluxo(cliente.Id, usuarioResponsavel.Id);
                AprovarOuEnviarParaControleDeRisco(cliente, usuarioResponsavel);
            }
        }

        #region Aprovação

        private void EnviarParaGerencia(Client cliente, User usuarioResponsavel)
        {
            if (cliente.CurrentStatus.Description != EStatus.EM_CADASTRO &&
                cliente.CurrentStatus.Description != EStatus.CORRECAO_PERFIL)
                throw new InvalidClientException("Cliente inválido para essa operação!");

            if (!usuarioResponsavel.Roles.Contains(ERoles.OPERACAO) &&
                !usuarioResponsavel.Roles.Contains(ERoles.ADMINISTRACAO))
                throw new InvalidUserException("Usuário inválido para essa operação!");

            _fluxoRepository.EnviarParaGerencia(cliente.Id, usuarioResponsavel.Id);
        }


        private void AprovarOuEnviarParaControleDeRisco(Client cliente, User usuarioResponsavel)
        {
            if (cliente.CurrentStatus.Description != EStatus.AGUARDANDO_GERENCIA)
                throw new InvalidClientException("Cliente inválido para essa operação!");

            if (!usuarioResponsavel.Roles.Contains(ERoles.GERENCIA) &&
                !usuarioResponsavel.Roles.Contains(ERoles.ADMINISTRACAO))
                throw new InvalidUserException("Usuário inválido para essa operação!");

            _fluxoRepository.EnviarParaAprovacao(cliente.Id, usuarioResponsavel.Id);
        }

        private void AprovarClienteInternacional(Client cliente, User usuarioResponsavel)
        {
            if (!cliente.IsInternacional ||
                cliente.CurrentStatus.Description != EStatus.AGUARDANDO_CONTROLE_DE_RISCO)
                throw new InvalidClientException("Cliente inválido para essa operação!");

            if (!usuarioResponsavel.Roles.Contains(ERoles.ADMINISTRACAO) &&
                !usuarioResponsavel.Roles.Contains(ERoles.CONTROLE_DE_RISCO))
                throw new InvalidUserException("Usuário inválido para essa operação!");

            _fluxoRepository.AprovarCliente(cliente.Id, usuarioResponsavel.Id);
        }

        #endregion

        public void ReprovarCliente(Client cliente, User usuarioResponsavel)
        {
            if (!usuarioResponsavel.Roles.Contains(ERoles.GERENCIA) &&
                !usuarioResponsavel.Roles.Contains(ERoles.ADMINISTRACAO) &&
                !usuarioResponsavel.Roles.Contains(ERoles.CONTROLE_DE_RISCO))
                throw new InvalidUserException("Usuário inválido para essa operação!");

            ValidateClientAndUser(cliente.Id, usuarioResponsavel.Id);
            ValidateFluxo(cliente.Id, usuarioResponsavel.Id);

            _fluxoRepository.ReprovarCliente(cliente.Id, usuarioResponsavel.Id);
        }

        public void EnviarParaCorrecao(Client cliente, User usuarioResponsavel)
        {
            if (!usuarioResponsavel.Roles.Contains(ERoles.GERENCIA) &&
                !usuarioResponsavel.Roles.Contains(ERoles.ADMINISTRACAO) &&
                !usuarioResponsavel.Roles.Contains(ERoles.CONTROLE_DE_RISCO))
                throw new InvalidUserException("Usuário inválido para essa operação!");

            ValidateClientAndUser(cliente.Id, usuarioResponsavel.Id);
            ValidateFluxo(cliente.Id, usuarioResponsavel.Id);

            _fluxoRepository.EnviarParaCorrecao(cliente.Id, usuarioResponsavel.Id);
        }

        #region Validações

        private void ValidateClientAndUser(int clientId, int userId)
        {
            var clientFromDb = _clientRepository.GetById(clientId);
            if (clientFromDb == null) throw new InvalidClientException("Cliente inexistente!");

            var user = _userRepository.GetById(userId);
            if (user == null) throw new InvalidUserException("Usuário inexistente!");
        }

        private void ValidateFluxo(int clientId, int userId)
        {
            var isUserIn = _fluxoRepository.VerifyIfUserInFluxo(clientId, userId);
            if (isUserIn)
                throw new InvalidUserException("Usuário inválido para essa operação!");
        }


        #endregion


    }
}
