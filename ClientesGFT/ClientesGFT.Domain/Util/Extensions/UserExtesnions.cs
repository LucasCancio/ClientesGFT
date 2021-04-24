using ClientesGFT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientesGFT.Domain.Util.Extensions
{
    public static class UserExtesnions
    {
        public static User FixIncompleteUser(this User user, User dbUser, out bool passwordChanged)
        {
            var roles = user.UserRoles ?? dbUser.UserRoles;

            var emptyPassword = string.IsNullOrEmpty(user.Password);
            passwordChanged = !(emptyPassword);

            string password = emptyPassword ? dbUser.Password : user.Password;

            return new User(dbUser.Id, user.Name, user.Login, password, dbUser.CreatedDate, roles);
        }
    }
}
