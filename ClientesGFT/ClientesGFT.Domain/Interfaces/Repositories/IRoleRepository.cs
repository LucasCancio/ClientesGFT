using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        List<Role> GetAll();
        ERoles[] GetById(int userId);
    }
}
