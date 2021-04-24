using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientesGFT.Data.EF.Repositories
{
    public class ClientEFRepository : IClientRepository
    {
        private readonly ClientesGFTContext _context;

        public ClientEFRepository(ClientesGFTContext context)
        {
            _context = context;
        }

        public IList<Client> GetAllByStatus(IList<int> idsStatus)
        {
            var clientsFromDb = _context.Clients
                                                .Include(cliente => cliente.Adress)
                                                .ThenInclude(endereco => endereco.City)
                                                .ThenInclude(cidade => cidade.State)
                                                .ThenInclude(estado => estado.Country)
                                                .Include(cliente => cliente.CurrentStatus)
                                                .OrderByDescending(x => x.Id)
                                                .Where(c => idsStatus.Contains(c.CurrentStatusId))
                                                .ToList();

            clientsFromDb.ForEach(x => x.SetIsInternacional());

            return clientsFromDb;
        }

        public Client GetById(int id, bool withPhones = false)
        {
            var query = _context.Clients
                                         .Include(cliente => cliente.Adress)
                                         .ThenInclude(endereco => endereco.City)
                                         .ThenInclude(cidade => cidade.State)
                                         .ThenInclude(estado => estado.Country)
                                         .Include(cliente => cliente.CurrentStatus);
            Client client = null;

            if (withPhones)
            {
                client = query.Include(cliente => cliente.Phones).AsNoTracking().FirstOrDefault(x => x.Id == id);
            }
            else
            {
                client = query.AsNoTracking().FirstOrDefault(x => x.Id == id);
            }

            client.SetIsInternacional();

            return client;
        }

        public bool VerifyIfHasSameData(Client client)
        {
            bool exists;
            if (client.Id > 0)
            {
                exists = _context.Clients.Where(x => x.Id != client.Id && x.CPF == client.CPF).Any();
            }
            else
            {
                exists = _context.Clients.Where(x => x.CPF == client.CPF).Any();
            }

            return exists;
        }

        private void EditPhones(Client client, List<Phone> phones)
        {
            var phonesInDb = _context.Phones.Where(p => p.ClientId == client.Id).ToList();

            var phonesToDelete = phonesInDb.Where(p => !phones.Contains(p)).ToList();
            var phonesToInsert = phones.Where(p => p.Id == 0).ToList();

            phonesToInsert.ForEach(p => p.ClientId = client.Id);

            _context.Phones.RemoveRange(phonesToDelete);
            _context.Phones.AddRange(phonesToInsert);
        }

        public void Update(Client client)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                EditPhones(client, client.Phones.ToList());

                _context.Entry(client).State = EntityState.Modified;
                _context.Entry(client.Adress).State = EntityState.Modified;

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public void Insert(Client client, User user)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Entry(client).State = EntityState.Added;
                _context.Entry(client.Adress).State = EntityState.Added;

                _context.Phones.AddRange(client.Phones);

                var fluxo = new Fluxo(client, user, client.CurrentStatusId);

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

        public void Delete(int clientId, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var fluxo = _context.Fluxos.First(x => x.ClientId == clientId && x.UserId == userId);
                var phones = _context.Phones.Where(x => x.ClientId == clientId);
                var clientFromDb = _context.Clients.Find(clientId);

                _context.Fluxos.Remove(fluxo);
                _context.Phones.RemoveRange(phones);
                _context.Clients.Remove(clientFromDb);

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }
}
