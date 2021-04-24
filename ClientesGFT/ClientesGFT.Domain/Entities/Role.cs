using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Util;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Entities
{
    public class Role
    {
        public Role(int id) : this()
        {
            Id = id;
            Description = EnumHelper.PerfilIdParaPerfil(id);
        }

        public Role(ERoles description) : this()
        {
            Description = description;
            Id = EnumHelper.PerfilParaPerfilId(description);
        }

        public Role()
        {
            UserRoles = new HashSet<UserRole>();
            Ativo = true;
        }

        public int Id { get; private set; }
        public ERoles Description { get; private set; }
        public string DisplayName { get => Description.GetDisplayName(); }

        public bool? Ativo { get; private set; }

        public virtual ICollection<UserRole> UserRoles { get; private set; }
    }
}
