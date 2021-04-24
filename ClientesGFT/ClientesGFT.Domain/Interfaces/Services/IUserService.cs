using ClientesGFT.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClientesGFT.Domain.Interfaces.Services
{
    public interface IUserService
    {
        User Login(string login, string senha);
        IList<User> GetAll(User loggedUser);
        User Get(int id);
        User Get(ClaimsPrincipal claim);
        List<Role> GetRoles();

        void Register(User user);
        void Edit(User user);
        void Delete(User user);

        List<Role> FixRoles(List<int> roleIds);
    }
}
