using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Interfaces.Repositories;
using ClientesGFT.Domain.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientesGFT.Data.EF.Repositories
{
    public class FluxoEFRepository : IFluxoRepository
    {
        private readonly ClientesGFTContext _context;
        private readonly IClientRepository _clientRepository;

        public FluxoEFRepository(ClientesGFTContext context, IClientRepository clientRepository)
        {
            _context = context;
            _clientRepository = clientRepository;
        }

        public IList<Fluxo> ListarFluxo(DateTime startDate, DateTime endDate, int statusId = 0, string cpf = "", string name = "")
        {
            var query = _context.Fluxos
                                       .Include(x => x.Client)
                                       .Include(x => x.User)
                                       .Include(x => x.Status)
                                       .Where(x => x.CreateDate.Date >= startDate.Date &&
                                                            x.CreateDate.Date <= endDate.Date);

            if (statusId > 0) query = query.Where(x => x.StatusId == statusId);

            if (!string.IsNullOrEmpty(name)) query = query.Where(x => x.Client.Name.Contains(name));

            if (!string.IsNullOrEmpty(cpf)) query = query.Where(x => x.Client.CPF == cpf);

            List<Fluxo> fluxos = query.OrderByDescending(x => x.CreateDate).ToList();

            return fluxos;
        }

        public IList<int> ListarIdsClientesIndisponiveis(int userId)
        {
            int idStatusEmCadastro = EnumHelper.StatusParaStatusId(EStatus.EM_CADASTRO);

            IList<int> clientIds = _context.Clients
                                                  .Select(x => x.Fluxos.OrderByDescending(x => x.Id).First())
                                                  .Where(x => x.StatusId == x.Client.CurrentStatusId &&
                                                              x.StatusId != idStatusEmCadastro &&
                                                              x.UserId == userId)
                                                  .Select(x => x.ClientId)
                                                  .ToList();


            return clientIds;
        }

        public void EnviarParaGerencia(int clientId, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var clientFromDb = _clientRepository.GetById(clientId);

                clientFromDb.SetCurrentStatus(EStatus.AGUARDANDO_GERENCIA);

                _context.Entry(clientFromDb).State = EntityState.Modified;

                var fluxo = new Fluxo(clientId, userId, clientFromDb.CurrentStatusId);

                _context.Entry(fluxo).State = EntityState.Added;

                _context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public void EnviarParaAprovacao(int clientId, int userId)
        {
            var clientFromDb = _clientRepository.GetById(clientId);

            if (clientFromDb.IsInternacional == true)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    clientFromDb.SetCurrentStatus(EStatus.AGUARDANDO_CONTROLE_DE_RISCO);

                    _context.Entry(clientFromDb).State = EntityState.Modified;

                    var fluxo = new Fluxo(clientId, userId, clientFromDb.CurrentStatusId);

                    _context.Entry(fluxo).State = EntityState.Added;

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            else
            {
                AprovarCliente(clientId, userId);
            }
        }

        public void AprovarCliente(int clientId, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var clientFromDb = _clientRepository.GetById(clientId);

                clientFromDb.SetCurrentStatus(EStatus.APROVADO);

                _context.Entry(clientFromDb).State = EntityState.Modified;

                var fluxo = new Fluxo(clientId, userId, clientFromDb.CurrentStatusId);

                _context.Entry(fluxo).State = EntityState.Added;

                _context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public void ReprovarCliente(int clientId, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var clientFromDb = _clientRepository.GetById(clientId);

                clientFromDb.SetCurrentStatus(EStatus.REPROVADO);

                _context.Entry(clientFromDb).State = EntityState.Modified;

                var fluxo = new Fluxo(clientId, userId, clientFromDb.CurrentStatusId);

                _context.Entry(fluxo).State = EntityState.Added;

                _context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public void EnviarParaCorrecao(int clientId, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var clientFromDb = _clientRepository.GetById(clientId);

                clientFromDb.SetCurrentStatus(EStatus.CORRECAO_PERFIL);

                _context.Entry(clientFromDb).State = EntityState.Modified;

                var fluxo = new Fluxo(clientId, userId, clientFromDb.CurrentStatusId);

                _context.Entry(fluxo).State = EntityState.Added;

                _context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public bool VerifyIfUserInFluxo(int clientId, int userId)
        {
            int idStatusEmCadastro = EnumHelper.StatusParaStatusId(EStatus.EM_CADASTRO);
            int idStatusCorrecaoPerfil = EnumHelper.StatusParaStatusId(EStatus.CORRECAO_PERFIL);
            var forbiddenIds = new List<int> { idStatusEmCadastro, idStatusCorrecaoPerfil };

            int lastUserIdFromClient = (from fluxo in _context.Fluxos
                                        join cliente in _context.Clients on fluxo.ClientId equals clientId
                                        where !forbiddenIds.Contains(fluxo.StatusId)
                                        orderby fluxo.Id descending
                                        select fluxo.UserId).FirstOrDefault();

            return userId == lastUserIdFromClient;
        }

    }
}
