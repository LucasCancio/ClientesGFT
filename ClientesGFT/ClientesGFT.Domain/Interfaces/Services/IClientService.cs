using ClientesGFT.Domain.Entities;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Interfaces.Services
{
    public interface IClientService
    {
        Client Get(int id, bool withPhones = false);
        IList<Client> GetAllToApproveClients(User user);
        IList<Client> GetAllFinalizedClients();
        IList<Client> GetAllOperationClients(User user);
        void Register(Client client, User user);
        void Edit(Client client);
        void Delete(Client client, User user);

        List<Phone> FixPhones(int clientId, List<string> phoneNumbers);

    }
}
