using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ClientesGFT.Data.EF.Repositories
{
    public class RoleEFRepository : IRoleRepository
    {
        private readonly ClientesGFTContext _context;

        public RoleEFRepository(ClientesGFTContext context)
        {
            _context = context;
        }

        public List<Role> GetAll()
        {
            return _context.Roles.ToList();
        }

        public ERoles[] GetById(int userId)
        {
            var roles = _context.UserRoles.Where(x => x.UserId == userId);

            return roles.Select(x => x.Role.Description).ToArray();
        }
    }
}
