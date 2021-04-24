using ClientesGFT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientesGFT.Domain.Entities
{
    public class User
    {
        public User(int id, string name, string login, string password, DateTime createdDate, ICollection<UserRole> userRoles = null)
            : this(name, login, password, userRoles)
        {
            Id = id;
            CreatedDate = createdDate;
        }

        public User(string name, string login, string password, ICollection<UserRole> userRoles = null)
            : this()
        {
            Name = name;
            Login = login;
            Password = password;

            CreatedDate = DateTime.Now;
            UserRoles = userRoles;
        }

        private User()
        {
            IsEnableToModify = true;
            Ativo = true;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public bool? Ativo { get; private set; }
        public virtual ICollection<Fluxo> Fluxos { get; private set; }
        public virtual ICollection<UserRole> UserRoles { get; private set; }

        public bool IsEnableToModify { get; private set; }
        public IList<ERoles> Roles { get; set; }

        public void ClearPassword() => this.Password = null;

        public void FixRoles()
        {
            var roles = this.UserRoles.Select(ur => ur.Role).Select(r => r.Description).ToList();
            this.Roles = roles;
        }

        public void DisableToModify()
        {
            this.IsEnableToModify = false;
        }

        public void Disable() => this.Ativo = false;
    }
}
