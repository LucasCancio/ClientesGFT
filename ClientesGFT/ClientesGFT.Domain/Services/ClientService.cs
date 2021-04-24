
using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Exceptions;
using ClientesGFT.Domain.Interfaces.Repositories;
using ClientesGFT.Domain.Interfaces.Services;
using ClientesGFT.Domain.Util;
using ClientesGFT.Domain.Util.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ClientesGFT.Domain.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clienteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFluxoRepository _fluxoRepository;

        public ClientService(IClientRepository clienteRepository, IFluxoRepository fluxoRepository, IUserRepository userRepository)
        {
            _clienteRepository = clienteRepository;
            _userRepository = userRepository;
            _fluxoRepository = fluxoRepository;
        }

        #region Listar clientes

        public IList<Client> GetAllToApproveClients(User user)
        {
            bool hasOnlyOperationRole = user.Roles.Count == 1 && user.Roles[0] == ERoles.OPERACAO;
            if (hasOnlyOperationRole) return new List<Client>();

            IList<EStatus> statusDisponiveis = new List<EStatus>();
            if (user.Roles.Contains(ERoles.CONTROLE_DE_RISCO) || user.Roles.Contains(ERoles.ADMINISTRACAO))
            {
                statusDisponiveis.Add(EStatus.AGUARDANDO_CONTROLE_DE_RISCO);
            }

            if (user.Roles.Contains(ERoles.GERENCIA) || user.Roles.Contains(ERoles.ADMINISTRACAO))
            {
                statusDisponiveis.Add(EStatus.AGUARDANDO_GERENCIA);
            }


            var idsStatus = statusDisponiveis
                                .Select(status => EnumHelper.StatusParaStatusId(status));

            IList<Client> clientes = _clienteRepository.GetAllByStatus(idsStatus.ToList());

            IList<int> clientesIdsIndiponiveis = _fluxoRepository.ListarIdsClientesIndisponiveis(user.Id);


            foreach (var cliente in clientes)
            {
                if (clientesIdsIndiponiveis.Contains(cliente.Id))
                    cliente.DisableToModify();
            }

            return clientes;
        }

        public IList<Client> GetAllFinalizedClients()
        {
            IList<EStatus> statusDisponiveis = new List<EStatus> { EStatus.APROVADO, EStatus.REPROVADO };

            var idsStatus = statusDisponiveis
                                .Select(status => EnumHelper.StatusParaStatusId(status));

            return _clienteRepository.GetAllByStatus(idsStatus.ToList());
        }

        public IList<Client> GetAllOperationClients(User user)
        {
            IList<EStatus> statusDisponiveis = new List<EStatus> { EStatus.CORRECAO_PERFIL, EStatus.EM_CADASTRO };

            var idsStatus = statusDisponiveis
                                .Select(status => EnumHelper.StatusParaStatusId(status));

            IList<Client> clientes = _clienteRepository.GetAllByStatus(idsStatus.ToList());

            IList<int> clientesIdsIndiponiveis = _fluxoRepository.ListarIdsClientesIndisponiveis(user.Id);

            foreach (var cliente in clientes)
            {
                if (clientesIdsIndiponiveis.Contains(cliente.Id))
                    cliente.DisableToModify();
            }

            return clientes;
        }

        #endregion


        public List<Phone> FixPhones(int clientId, List<string> phoneNumbers)
        {
            var fixedPhones = new List<Phone>();

            if (clientId > 0)
            {
                List<Phone> phonesInDb = _clienteRepository.GetById(clientId)?.Phones.ToList();
                List<Phone> phonesToFix = phoneNumbers.Select(number => new Phone(number)).ToList();

                phonesInDb.ForEach(phone =>
                {
                    if (phoneNumbers.Contains(phone.Number))
                    {
                        fixedPhones.Add(phone);
                        phoneNumbers.Remove(phone.Number);
                    }
                });
            }

            phoneNumbers.ForEach(number => fixedPhones.Add(new Phone(number)));

            return fixedPhones;
        }

        #region Crud

        public Client Get(int id, bool withPhones = false)
        {
            Client client = _clienteRepository.GetById(id, withPhones);
            return client;
        }

        public void Register(Client client, User user)
        {
            if (_clienteRepository.VerifyIfHasSameData(client))
                throw new InvalidClientException("Cliente já existe!");

            var userFromDb = _userRepository.GetById(user.Id);
            if (userFromDb == null) throw new InvalidUserException("Usuário inexistente!");


            _clienteRepository.Insert(client, userFromDb);
        }

        public void Edit(Client client)
        {
            var clientInDb = _clienteRepository.GetById(client.Id);

            if (clientInDb.CurrentStatus.Description != EStatus.EM_CADASTRO &&
                clientInDb.CurrentStatus.Description != EStatus.CORRECAO_PERFIL)
            {
                throw new InvalidClientException("Cliente inválido para essa operação!");
            }

            if (_clienteRepository.VerifyIfHasSameData(client))
                throw new InvalidClientException("Cliente já existe!");

            var fixedClient = client.FixIncompleteClient(clientInDb);

            _clienteRepository.Update(fixedClient);
        }

        public void Delete(Client client, User user)
        {
            if (client.CurrentStatus.Description != EStatus.EM_CADASTRO)
                throw new InvalidClientException("Cliente inválido para essa operação!");

            if (!user.Roles.Contains(ERoles.OPERACAO) &&
                !user.Roles.Contains(ERoles.ADMINISTRACAO))
                throw new InvalidUserException("Usuário inválido para essa operação!");

            var userFromDb = _userRepository.GetById(user.Id);
            if (userFromDb == null) throw new InvalidUserException("Usuário inexistente!");

            _clienteRepository.Delete(client.Id, user.Id);
        }

        #endregion
    }
}
