using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Interfaces.Repositories
{
    public interface IClientRepository
    {
        IList<Client> GetAllByStatus(IList<int> idsStatus);
        Client GetById(int id, bool withPhones = false);
        void Insert(Client client, User user);
        void Update(Client client);
        void Delete(int clientId, int userId);

        bool VerifyIfHasSameData(Client client);
    }
}
