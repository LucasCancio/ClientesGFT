using ClientesGFT.Domain.Entities;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        IList<User> GetAll();
        User GetById(int id);
        User GetByCredentials(string login, string password);

        void Insert(User user);
        void Update(User user, bool passwordChanged);
        void Delete(int userId);
    }
}
